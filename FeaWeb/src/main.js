import './styles/main.css';
import './styles/ribbon.css';
import './styles/panels.css';
import './styles/dialogs.css';

import { Viewport }         from './viewport/Viewport.js';
import { Ribbon }           from './ui/Ribbon.js';
import { ModelTree }        from './ui/ModelTree.js';
import { PropertiesPanel }  from './ui/PropertiesPanel.js';
import { StatusBar }        from './ui/StatusBar.js';
import { ToolManager }      from './tools/ToolManager.js';
import { SelectTool }       from './tools/SelectTool.js';
import { AddNodeTool }      from './tools/AddNodeTool.js';
import { AddBeamTool }      from './tools/AddBeamTool.js';
import { AddSupportTool, AddLoadTool } from './tools/AddSupportTool.js';
import { AnalysisEngine }   from './engine/AnalysisEngine.js';
import { eventBus }         from './core/EventBus.js';
import { appState }         from './core/AppState.js';

/* ── Bootstrap ─────────────────────────────────────────── */
window.addEventListener('DOMContentLoaded', () => {
  const canvas = document.getElementById('viewport-canvas');

  /* 3D Viewport */
  const vp = new Viewport(canvas);

  /* Tool Manager */
  const toolMgr = new ToolManager(vp);
  toolMgr.register('select',      new SelectTool(vp));
  toolMgr.register('add-node',    new AddNodeTool(vp));
  toolMgr.register('add-beam',    new AddBeamTool(vp));
  toolMgr.register('add-support', new AddSupportTool(vp));
  toolMgr.register('add-load',    new AddLoadTool(vp));

  /* UI Components */
  const ribbon  = new Ribbon(document.getElementById('ribbon'), vp, toolMgr);
  const tree    = new ModelTree(document.getElementById('model-tree-panel'));
  const props   = new PropertiesPanel(document.getElementById('properties-panel'));
  const status  = new StatusBar(document.getElementById('status-bar'));

  /* Mouse move → status bar coords */
  canvas.addEventListener('mousemove', e => {
    const snap = vp.snapping.snap(e);
    if (snap) eventBus.emit('cursor:moved', snap);
  });

  /* Hint messages → status bar */
  eventBus.on('status:hint', msg => {
    const el = document.getElementById('sb-status');
    if (el) el.textContent = msg;
  });

  /* Delete selection */
  eventBus.on('action:delete-selection', () => {
    for (const sel of appState.selection) {
      if (sel.type === 'node')    appState.removeNode(sel.id);
      if (sel.type === 'element') appState.removeElement(sel.id);
      if (sel.type === 'support') { appState.model.removeSupport(sel.id); eventBus.emit('model:changed'); }
    }
    appState.clearSelection();
  });

  /* Run Analysis */
  const engine = new AnalysisEngine();
  eventBus.on('action:run-analysis', () => {
    eventBus.emit('analysis:running');
    try {
      const lcId = appState.activeLoadCaseId;
      const results = engine.analyze(appState.model, lcId);
      appState.setResults(results);
      showToast('Analysis complete!', 'success');
    } catch (err) {
      eventBus.emit('analysis:error', err.message);
      showToast(err.message, 'error');
    }
  });

  /* Keyboard shortcut help */
  eventBus.emit('status:hint', 'Ready — N: Node | B: Beam | S: Select | Del: Delete | Ctrl+Z/Y: Undo/Redo');

  /* Seed demo model */
  _seedDemo(appState);

  /* Initial render */
  vp.refreshAll();
  setTimeout(() => vp.fitAll(), 200);
});

/* ── Toast notifications ──────────────────────────────── */
function showToast(msg, type = 'info') {
  const icons = {
    success: `<svg viewBox="0 0 16 16" fill="none" stroke="var(--accent-green)" stroke-width="1.5"><polyline points="2,8 6,12 14,4"/></svg>`,
    error:   `<svg viewBox="0 0 16 16" fill="none" stroke="var(--accent-red)" stroke-width="1.5"><line x1="4" y1="4" x2="12" y2="12"/><line x1="12" y1="4" x2="4" y2="12"/></svg>`,
    info:    `<svg viewBox="0 0 16 16" fill="none" stroke="var(--accent-blue)" stroke-width="1.5"><circle cx="8" cy="8" r="6"/><line x1="8" y1="6" x2="8" y2="10"/></svg>`,
  };
  const container = document.getElementById('toast-container');
  const toast = document.createElement('div');
  toast.className = `toast ${type}`;
  toast.innerHTML = `${icons[type] || icons.info} <span>${msg}</span>`;
  container.appendChild(toast);
  setTimeout(() => toast.remove(), 3500);
}

/* ── Demo Model ─────────────────────────────────────────── */
function _seedDemo(state) {
  // Simple portal frame: 2 columns, 1 beam, fixed bases
  const n1 = state.addNode(0, 0, 0);
  const n2 = state.addNode(0, 4, 0);
  const n3 = state.addNode(6, 4, 0);
  const n4 = state.addNode(6, 0, 0);
  state.addElement(n1.id, n2.id); // left column
  state.addElement(n2.id, n3.id); // beam
  state.addElement(n3.id, n4.id); // right column
  state.addSupport(n1.id, 'fixed');
  state.addSupport(n4.id, 'fixed');
  state.addNodeLoad(n2.id, 0, -20000, 0); // 20kN down at left top
  state.addNodeLoad(n3.id, 0, -20000, 0); // 20kN down at right top
}
