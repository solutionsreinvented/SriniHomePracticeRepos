import { eventBus } from '../core/EventBus.js';
import { appState }  from '../core/AppState.js';

export class ToolManager {
  constructor(viewport) {
    this.vp      = viewport;
    this._active = null;
    this._tools  = {};

    // Keyboard shortcuts
    window.addEventListener('keydown', e => this._onKey(e));
  }

  register(name, tool) { this._tools[name] = tool; }

  activate(name, opts = {}) {
    if (this._active) {
      this._active.deactivate();
      this._active = null;
    }
    const tool = this._tools[name];
    if (!tool) return;
    tool.activate(opts);
    this._active = tool;
    appState.activeTool = tool;
    eventBus.emit('tool:changed', name);
  }

  deactivate() {
    if (this._active) { this._active.deactivate(); this._active = null; }
    appState.activeTool = null;
    eventBus.emit('tool:changed', null);
  }

  _onKey(e) {
    if (e.target.tagName === 'INPUT') return;
    switch (e.key) {
      case 'Escape': this.deactivate(); break;
      case 'n': this.activate('add-node'); break;
      case 'b': this.activate('add-beam'); break;
      case 's': this.activate('select');   break;
      case 'Delete': eventBus.emit('action:delete-selection'); break;
    }
    if (e.ctrlKey && e.key === 'z') { e.preventDefault(); appState.history.undo(); }
    if (e.ctrlKey && e.key === 'y') { e.preventDefault(); appState.history.redo(); }
  }
}
