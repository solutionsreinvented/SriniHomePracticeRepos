import { eventBus } from '../core/EventBus.js';
import { appState }  from '../core/AppState.js';

export class StatusBar {
  constructor(el) {
    this.el = el;
    this._coordX = el.querySelector('#sb-x');
    this._coordY = el.querySelector('#sb-y');
    this._coordZ = el.querySelector('#sb-z');
    this._tool   = el.querySelector('#status-tool-name');
    this._mode   = el.querySelector('#status-mode');
    this._nodes  = el.querySelector('#sb-nodes');
    this._elems  = el.querySelector('#sb-elems');
    this._status = el.querySelector('#sb-status');

    eventBus.on('cursor:moved',     p  => this._updateCoords(p));
    eventBus.on('tool:changed',     t  => this._updateTool(t));
    eventBus.on('model:changed',    () => this._updateStats());
    eventBus.on('analysis:complete',() => this._setStatus('Analysis complete ✓', 'green'));
    eventBus.on('analysis:running', () => this._setStatus('Running analysis…', 'yellow'));
    eventBus.on('analysis:error',  e  => this._setStatus(`Error: ${e}`, 'red'));
    this._updateStats();
  }

  _updateCoords(pt) {
    if (!pt) return;
    const fmt = v => v.toFixed(3).padStart(8);
    if (this._coordX) this._coordX.textContent = fmt(pt.x);
    if (this._coordY) this._coordY.textContent = fmt(pt.y);
    if (this._coordZ) this._coordZ.textContent = fmt(pt.z);
  }

  _updateTool(toolName) {
    if (this._tool) this._tool.textContent = toolName || 'Select';
  }

  _updateStats() {
    const s = appState.model.stats;
    if (this._nodes) this._nodes.textContent = s.nodes;
    if (this._elems) this._elems.textContent = s.elements;
  }

  _setStatus(msg, color = 'default') {
    if (!this._status) return;
    const colors = { green:'var(--accent-green)', yellow:'var(--accent-yellow)', red:'var(--accent-red)', default:'var(--text-secondary)' };
    this._status.textContent = msg;
    this._status.style.color = colors[color] || colors.default;
  }
}
