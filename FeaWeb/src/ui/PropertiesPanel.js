import { eventBus } from '../core/EventBus.js';
import { appState }  from '../core/AppState.js';

function row(label, value, unit = '', id = '') {
  return `<div class="props-row">
    <label>${label}</label>
    <input class="fea-input" type="number" value="${value}" step="any" data-prop="${id || label.toLowerCase()}" />
    <span class="unit">${unit}</span>
  </div>`;
}

function roText(label, value) {
  return `<div class="props-row">
    <label>${label}</label>
    <span style="font-family:var(--font-mono);font-size:12px;color:var(--accent-cyan)">${value}</span>
  </div>`;
}

function renderNode(node) {
  return `
    <div class="props-type-badge props-type-node">● Node ${node.id}</div>
    <div class="props-section">
      <div class="props-section-title">Coordinates</div>
      ${row('X', node.x.toFixed(4), 'm', 'x')}
      ${row('Y', node.y.toFixed(4), 'm', 'y')}
      ${row('Z', node.z.toFixed(4), 'm', 'z')}
    </div>
    <div class="props-section">
      <div class="props-section-title">Label</div>
      <div class="props-row">
        <label>Name</label>
        <input class="fea-input" type="text" value="${node.label}" data-prop="label" />
      </div>
    </div>
    <div class="props-apply-row">
      <button class="fea-btn fea-btn-primary" id="props-apply">Apply</button>
    </div>`;
}

function renderElement(el, model) {
  const n1 = model.nodes.get(el.node1Id);
  const n2 = model.nodes.get(el.node2Id);
  const sec = model.sections.get(el.sectionId);
  return `
    <div class="props-type-badge props-type-beam">▬ Member ${el.id}</div>
    <div class="props-section">
      <div class="props-section-title">Connectivity</div>
      ${roText('Start Node', n1 ? n1.label : '—')}
      ${roText('End Node',   n2 ? n2.label : '—')}
      ${roText('Length', n1 && n2
        ? (Math.sqrt((n2.x-n1.x)**2+(n2.y-n1.y)**2+(n2.z-n1.z)**2)).toFixed(3)+' m' : '—')}
    </div>
    <div class="props-section">
      <div class="props-section-title">Section</div>
      ${roText('Section', sec ? sec.name : '—')}
      ${sec ? roText('Area', sec.A.toExponential(3)+' m²') : ''}
    </div>`;
}

function renderSupport(sup, model) {
  const node = model.nodes.get(sup.nodeId);
  const dof  = ['ux','uy','uz','rx','ry','rz'];
  return `
    <div class="props-type-badge props-type-support">▲ Support ${sup.id}</div>
    <div class="props-section">
      <div class="props-section-title">Location</div>
      ${roText('Node', node ? node.label : '—')}
      ${roText('Type', sup.type)}
    </div>
    <div class="props-section">
      <div class="props-section-title">Restraints</div>
      ${dof.map(d => `<div class="props-row">
        <label>${d.toUpperCase()}</label>
        <span style="color:${sup[d] ? 'var(--accent-green)' : 'var(--text-muted)'};font-weight:600">
          ${sup[d] ? '✓ Fixed' : '○ Free'}
        </span>
      </div>`).join('')}
    </div>`;
}

function renderLoad(load, model) {
  const node = load.nodeId ? model.nodes.get(load.nodeId) : null;
  return `
    <div class="props-type-badge props-type-load">↓ Load ${load.id}</div>
    <div class="props-section">
      <div class="props-section-title">Application</div>
      ${roText('Type', load.type)}
      ${roText('Load Case', load.loadCaseId)}
      ${node ? roText('Node', node.label) : ''}
    </div>
    <div class="props-section">
      <div class="props-section-title">Force (kN)</div>
      ${row('Fx', (load.Fx/1000).toFixed(2), 'kN', 'Fx')}
      ${row('Fy', (load.Fy/1000).toFixed(2), 'kN', 'Fy')}
      ${row('Fz', (load.Fz/1000).toFixed(2), 'kN', 'Fz')}
    </div>
    <div class="props-apply-row">
      <button class="fea-btn fea-btn-primary" id="props-apply">Apply</button>
    </div>`;
}

const EMPTY = `<div class="props-empty">
  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1">
    <rect x="3" y="3" width="18" height="18" rx="2"/>
    <path d="M9 9h6M9 12h6M9 15h4"/>
  </svg>
  <p>Select an entity<br/>to view its properties</p>
</div>`;

export class PropertiesPanel {
  constructor(container) {
    this.el = container;
    this._renderEmpty();
    eventBus.on('selection:changed', ({ selection }) => this._onSelection(selection));
    eventBus.on('model:changed', () => this._refresh());
  }

  _renderEmpty() {
    const scroll = this.el.querySelector('#properties-scroll');
    if (scroll) scroll.innerHTML = EMPTY;
  }

  _onSelection(selection) {
    this._currentSel = [...selection][0] || null;
    this._refresh();
  }

  _refresh() {
    const scroll = this.el.querySelector('#properties-scroll');
    if (!scroll) return;
    if (!this._currentSel) { scroll.innerHTML = EMPTY; return; }

    const { type, id } = this._currentSel;
    const m = appState.model;
    let html = '';

    if (type === 'node')    html = renderNode(m.nodes.get(id));
    else if (type === 'element') html = renderElement(m.elements.get(id), m);
    else if (type === 'support') html = renderSupport(m.supports.get(id), m);
    else if (type === 'load')    html = renderLoad(m.loads.get(id), m);
    else { scroll.innerHTML = EMPTY; return; }

    scroll.innerHTML = html;

    // Apply button
    const applyBtn = scroll.querySelector('#props-apply');
    if (applyBtn) applyBtn.addEventListener('click', () => this._applyChanges(type, id, scroll));
  }

  _applyChanges(type, id, scroll) {
    const m = appState.model;
    if (type === 'node') {
      const n = m.nodes.get(id);
      if (!n) return;
      scroll.querySelectorAll('[data-prop]').forEach(inp => {
        const prop = inp.dataset.prop;
        if (prop === 'label') n.label = inp.value;
        else n[prop] = parseFloat(inp.value) || 0;
      });
      eventBus.emit('model:changed');
    } else if (type === 'load') {
      const l = m.loads.get(id);
      if (!l) return;
      scroll.querySelectorAll('[data-prop]').forEach(inp => {
        const prop = inp.dataset.prop;
        l[prop] = (parseFloat(inp.value) || 0) * 1000; // kN → N
      });
      eventBus.emit('model:changed');
    }
  }
}
