import { eventBus } from '../core/EventBus.js';
import { appState }  from '../core/AppState.js';

const catIcon = {
  nodes:     `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><circle cx="8" cy="8" r="3"/></svg>`,
  elements:  `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><line x1="2" y1="14" x2="14" y2="2"/></svg>`,
  supports:  `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><polygon points="8,2 15,14 1,14"/></svg>`,
  loads:     `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><line x1="8" y1="2" x2="8" y2="12"/><polyline points="4,8 8,12 12,8"/></svg>`,
  loadcases: `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><rect x="2" y="2" width="12" height="12" rx="1"/><polyline points="5,8 7,10 11,6"/></svg>`,
  materials: `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><rect x="2" y="2" width="12" height="12" rx="1"/></svg>`,
  sections:  `<svg viewBox="0 0 16 16" fill="none" stroke="currentColor" stroke-width="1.5"><rect x="5" y="1" width="6" height="3"/><rect x="6" y="4" width="4" height="8"/><rect x="5" y="12" width="6" height="3"/></svg>`,
};

const chevron = `<svg class="tree-chevron" viewBox="0 0 12 12" fill="none" stroke="currentColor" stroke-width="1.5"><polyline points="2,4 6,8 10,4"/></svg>`;

export class ModelTree {
  constructor(container) {
    this.el = container;
    this._collapsed = {};
    this._render();
    eventBus.on('model:changed', () => this._render());
    eventBus.on('selection:changed', () => this._syncSelection());
  }

  _render() {
    const m = appState.model;
    const scroll = this.el.querySelector('#model-tree-scroll');
    if (!scroll) return;

    const cats = [
      { key: 'nodes',     label: 'Nodes',      items: [...m.nodes.values()],     dotClass: 'dot-node'    },
      { key: 'elements',  label: 'Members',     items: [...m.elements.values()],  dotClass: 'dot-beam'    },
      { key: 'supports',  label: 'Supports',    items: [...m.supports.values()],  dotClass: 'dot-support' },
      { key: 'loads',     label: 'Loads',       items: [...m.loads.values()],     dotClass: 'dot-load'    },
      { key: 'loadcases', label: 'Load Cases',  items: [...m.loadCases.values()], dotClass: 'dot-lcase'   },
      { key: 'materials', label: 'Materials',   items: [...m.materials.values()], dotClass: 'dot-node'    },
      { key: 'sections',  label: 'Sections',    items: [...m.sections.values()],  dotClass: 'dot-beam'    },
    ];

    scroll.innerHTML = cats.map(cat => `
      <div class="tree-category${this._collapsed[cat.key] ? ' collapsed' : ''}" data-cat="${cat.key}">
        <div class="tree-category-header">
          ${catIcon[cat.key] || ''}
          ${cat.label}
          <span class="tree-category-count">(${cat.items.length})</span>
          ${chevron}
        </div>
        <div class="tree-children">
          ${cat.items.map(item => `
            <div class="tree-item" data-type="${this._typeOf(cat.key)}" data-id="${item.id}">
              <span class="dot ${cat.dotClass}"></span>
              ${item.label || item.name || `${cat.label.slice(0,-1)} ${item.id}`}
            </div>`).join('')}
        </div>
      </div>`).join('');

    // Bind
    scroll.querySelectorAll('.tree-category-header').forEach(h => {
      h.addEventListener('click', () => {
        const cat = h.parentElement.dataset.cat;
        this._collapsed[cat] = !this._collapsed[cat];
        h.parentElement.classList.toggle('collapsed', this._collapsed[cat]);
      });
    });
    scroll.querySelectorAll('.tree-item').forEach(item => {
      item.addEventListener('click', e => {
        e.stopPropagation();
        const type = item.dataset.type;
        const id   = parseInt(item.dataset.id);
        appState.selectSingle(type, id);
      });
    });
  }

  _typeOf(cat) {
    const map = { nodes:'node', elements:'element', supports:'support',
                  loads:'load', loadcases:'loadcase', materials:'material', sections:'section' };
    return map[cat] || cat;
  }

  _syncSelection() {
    this.el.querySelectorAll('.tree-item').forEach(item => {
      const type = item.dataset.type;
      const id   = parseInt(item.dataset.id);
      const sel  = [...appState.selection].some(s => s.type === type && s.id === id);
      item.classList.toggle('selected', sel);
    });
  }
}
