import { eventBus } from '../core/EventBus.js';
import { appState }  from '../core/AppState.js';

/* ── SVG icon helper ─────────────────────────────── */
const icons = {
  node:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><circle cx="12" cy="12" r="4"/><circle cx="4" cy="4" r="2"/><circle cx="20" cy="4" r="2"/><circle cx="4" cy="20" r="2"/><circle cx="20" cy="20" r="2"/></svg>`,
  beam:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><line x1="3" y1="21" x2="21" y2="3"/><circle cx="3" cy="21" r="2.5" fill="currentColor"/><circle cx="21" cy="3" r="2.5" fill="currentColor"/></svg>`,
  support:   `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><polygon points="12,4 22,20 2,20"/><line x1="2" y1="22" x2="22" y2="22"/></svg>`,
  load:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><line x1="12" y1="3" x2="12" y2="17"/><polyline points="7,12 12,17 17,12"/></svg>`,
  material:  `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="3" y="3" width="18" height="18" rx="2"/><path d="M3 9h18M3 15h18M9 3v18M15 3v18"/></svg>`,
  section:   `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="8" y="2" width="8" height="4"/><rect x="10" y="6" width="4" height="12"/><rect x="8" y="18" width="8" height="4"/></svg>`,
  loadcase:  `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M9 11l3 3L22 4"/><path d="M21 12v7a2 2 0 01-2 2H5a2 2 0 01-2-2V5a2 2 0 012-2h11"/></svg>`,
  run:       `<svg viewBox="0 0 24 24" fill="currentColor"><polygon points="5,3 19,12 5,21"/></svg>`,
  select:    `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M4 4l7 18 3-7 7-3z"/></svg>`,
  delete:    `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><polyline points="3,6 5,6 21,6"/><path d="M19 6l-1 14H6L5 6"/><path d="M10 11v6M14 11v6"/><path d="M9 6V4h6v2"/></svg>`,
  fit:       `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M15 3h6v6M9 21H3v-6M21 3l-7 7M3 21l7-7"/></svg>`,
  front:     `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="3" y="3" width="18" height="18" rx="2"/><line x1="12" y1="3" x2="12" y2="21"/><line x1="3" y1="12" x2="21" y2="12"/></svg>`,
  iso:       `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M12 2l9 5v10l-9 5-9-5V7z"/><line x1="12" y1="2" x2="12" y2="22"/><line x1="3" y1="7" x2="21" y2="17"/><line x1="21" y1="7" x2="3" y2="17"/></svg>`,
  undo:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M3 7v6h6"/><path d="M3 13A9 9 0 1 0 21 12"/></svg>`,
  redo:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M21 7v6h-6"/><path d="M21 13A9 9 0 1 1 3 12"/></svg>`,
  grid:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="3" y="3" width="7" height="7"/><rect x="14" y="3" width="7" height="7"/><rect x="3" y="14" width="7" height="7"/><rect x="14" y="14" width="7" height="7"/></svg>`,
  deform:    `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M3 18 Q6 6 12 10 Q18 14 21 6"/><path d="M3 18 L21 18" stroke-dasharray="3 2" opacity=".4"/></svg>`,
  pinned:    `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><polygon points="12,4 21,20 3,20"/><line x1="3" y1="22" x2="21" y2="22"/></svg>`,
  fixed:     `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><rect x="3" y="16" width="18" height="6"/><line x1="9" y1="16" x2="6" y2="22"/><line x1="14" y1="16" x2="11" y2="22"/><line x1="19" y1="16" x2="16" y2="22"/><line x1="12" y1="3" x2="12" y2="16"/></svg>`,
  roller:    `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><polygon points="12,4 21,18 3,18"/><circle cx="7" cy="21" r="1.5" fill="currentColor"/><circle cx="12" cy="21" r="1.5" fill="currentColor"/><circle cx="17" cy="21" r="1.5" fill="currentColor"/></svg>`,
  udl:       `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><line x1="3" y1="20" x2="21" y2="20"/><line x1="6" y1="8" x2="6" y2="20"/><line x1="12" y1="8" x2="12" y2="20"/><line x1="18" y1="8" x2="18" y2="20"/><line x1="3" y1="8" x2="21" y2="8"/></svg>`,
  pointload: `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><line x1="12" y1="4" x2="12" y2="18"/><polyline points="7,13 12,18 17,13"/><line x1="7" y1="4" x2="17" y2="4"/></svg>`,
  save:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v11a2 2 0 01-2 2z"/><polyline points="17 21 17 13 7 13 7 21"/><polyline points="7 3 7 8 15 8"/></svg>`,
  open:      `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M22 19a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2h5l2 3h9a2 2 0 012 2z"/></svg>`,
  new:       `<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8"><path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z"/><polyline points="14 2 14 8 20 8"/><line x1="12" y1="11" x2="12" y2="17"/><line x1="9" y1="14" x2="15" y2="14"/></svg>`,
};

function btn(id, iconKey, label, cls = '') {
  return `<button class="ribbon-btn ${cls}" id="rbtn-${id}" title="${label}" data-tool="${id}">
    ${icons[iconKey] || icons.node}
    <span>${label}</span>
  </button>`;
}

const TABS = [
  { id: 'model',    label: 'Model'    },
  { id: 'loads',    label: 'Loads'    },
  { id: 'analysis', label: 'Analysis' },
  { id: 'design',   label: 'Design'   },
  { id: 'view',     label: 'View'     },
];

const CONTENT = {
  model: `
    <div class="ribbon-group">
      ${btn('select','select','Select')}
    </div>
    <div class="ribbon-group">
      ${btn('add-node','node','Add Node')}
      ${btn('add-beam','beam','Add Member')}
    </div>
    <div class="ribbon-group">
      ${btn('add-pinned','pinned','Pinned')}
      ${btn('add-fixed','fixed','Fixed')}
      ${btn('add-roller','roller','Roller')}
    </div>
    <div class="ribbon-group">
      ${btn('add-material','material','Material')}
      ${btn('add-section','section','Section')}
    </div>
    <div class="ribbon-group">
      ${btn('delete-sel','delete','Delete')}
      <div class="ribbon-sep"></div>
      ${btn('undo','undo','Undo')}
      ${btn('redo','redo','Redo')}
    </div>`,
  loads: `
    <div class="ribbon-group">
      ${btn('add-loadcase','loadcase','Load Case')}
    </div>
    <div class="ribbon-group">
      ${btn('add-nodeload','pointload','Node Force')}
      ${btn('add-udl','udl','Member UDL')}
    </div>`,
  analysis: `
    <div class="ribbon-group">
      ${btn('run-analysis','run','Run Analysis','lg run-analysis')}
    </div>
    <div class="ribbon-group">
      ${btn('show-deformed','deform','Deformed')}
    </div>`,
  design: `
    <div class="ribbon-group">
      <button class="ribbon-btn" title="Steel Design (coming soon)" disabled>
        ${icons.section}<span>Steel</span>
      </button>
    </div>`,
  view: `
    <div class="ribbon-group">
      ${btn('view-iso','iso','3D Iso')}
      ${btn('view-front','front','Front')}
      ${btn('view-top','grid','Top')}
    </div>
    <div class="ribbon-group">
      ${btn('fit-all','fit','Fit All')}
    </div>
    <div class="ribbon-group">
      ${btn('toggle-grid','grid','Grid')}
      ${btn('toggle-labels','node','Labels')}
    </div>`,
};

export class Ribbon {
  constructor(container, viewport, toolManager) {
    this.el       = container;
    this.vp       = viewport;
    this.toolMgr  = toolManager;
    this._activeTab = 'model';
    this._render();
    this._bind();
    eventBus.on('tool:changed', t => this._syncToolBtn(t));
    eventBus.on('history:changed', h => this._syncHistory(h));
  }

  _render() {
    this.el.innerHTML = `
      <div class="ribbon-tabs">
        ${TABS.map(t => `<div class="ribbon-tab${t.id === this._activeTab ? ' active' : ''}" data-tab="${t.id}">${t.label}</div>`).join('')}
      </div>
      ${TABS.map(t => `
        <div class="ribbon-content${t.id === this._activeTab ? ' active' : ''}" data-content="${t.id}">
          ${CONTENT[t.id] || ''}
        </div>`).join('')}
    `;
  }

  _bind() {
    // Tab switching
    this.el.querySelectorAll('.ribbon-tab').forEach(tab => {
      tab.addEventListener('click', () => {
        this._activeTab = tab.dataset.tab;
        this.el.querySelectorAll('.ribbon-tab').forEach(t => t.classList.toggle('active', t.dataset.tab === this._activeTab));
        this.el.querySelectorAll('.ribbon-content').forEach(c => c.classList.toggle('active', c.dataset.content === this._activeTab));
      });
    });

    // Tool buttons
    this.el.querySelectorAll('.ribbon-btn[data-tool]').forEach(btn => {
      btn.addEventListener('click', () => this._handleAction(btn.dataset.tool));
    });
  }

  _handleAction(action) {
    const vp = this.vp;
    switch (action) {
      case 'select':      this.toolMgr.activate('select');   break;
      case 'add-node':    this.toolMgr.activate('add-node'); break;
      case 'add-beam':    this.toolMgr.activate('add-beam'); break;
      case 'add-pinned':  this.toolMgr.activate('add-support', { type: 'pinned' }); break;
      case 'add-fixed':   this.toolMgr.activate('add-support', { type: 'fixed'  }); break;
      case 'add-roller':  this.toolMgr.activate('add-support', { type: 'roller-x' }); break;
      case 'add-nodeload':this.toolMgr.activate('add-load'); break;
      case 'add-udl':     this.toolMgr.activate('add-udl'); break;
      case 'delete-sel':  eventBus.emit('action:delete-selection'); break;
      case 'undo':        appState.history.undo(); break;
      case 'redo':        appState.history.redo(); break;
      case 'run-analysis':eventBus.emit('action:run-analysis'); break;
      case 'show-deformed':
        appState.displaySettings.showDeformed = !appState.displaySettings.showDeformed;
        eventBus.emit('model:changed');
        break;
      case 'view-iso':   vp.setViewDirection('iso');   break;
      case 'view-front': vp.setViewDirection('front'); break;
      case 'view-top':   vp.setViewDirection('top');   break;
      case 'fit-all':    vp.fitAll(); break;
      case 'toggle-grid':
        appState.displaySettings.showGrid = !appState.displaySettings.showGrid;
        break;
    }
  }

  _syncToolBtn(toolName) {
    this.el.querySelectorAll('.ribbon-btn[data-tool]').forEach(b => {
      b.dataset.active = b.dataset.tool === toolName ? 'true' : 'false';
    });
  }

  _syncHistory({ canUndo, canRedo }) {
    const u = this.el.querySelector('#rbtn-undo');
    const r = this.el.querySelector('#rbtn-redo');
    if (u) u.disabled = !canUndo;
    if (r) r.disabled = !canRedo;
  }
}
