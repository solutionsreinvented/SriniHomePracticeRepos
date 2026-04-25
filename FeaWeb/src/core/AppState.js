import { FEAModel }       from '../models/FEAModel.js';
import { eventBus }       from './EventBus.js';
import { CommandHistory } from './CommandHistory.js';

class AppState {
  constructor() {
    this.model          = new FEAModel();
    this.history        = new CommandHistory();
    this.activeTool     = null;
    this.selection      = new Set();       // Set of {type, id}
    this.activeLoadCaseId = 1;
    this.analysisResults  = null;
    this.displaySettings  = {
      showNodeLabels:    true,
      showMemberLabels:  false,
      showSupports:      true,
      showLoads:         true,
      showDeformed:      false,
      showBMD:           false,
      showSFD:           false,
      deformedScale:     100,
      renderMode:        'solid', // solid | wireframe
    };
    this.activeSectionId  = 1;
    this.activeMaterialId = 1;
  }

  /* Selection helpers */
  selectSingle(type, id) {
    this.selection = new Set([{ type, id }]);
    eventBus.emit('selection:changed', { selection: this.selection });
  }
  addToSelection(type, id) {
    this.selection.add({ type, id });
    eventBus.emit('selection:changed', { selection: this.selection });
  }
  clearSelection() {
    this.selection = new Set();
    eventBus.emit('selection:changed', { selection: this.selection });
  }
  getFirstSelected() {
    return this.selection.size ? [...this.selection][0] : null;
  }

  /* Model mutation helpers that emit events */
  addNode(x, y, z) {
    const n = this.model.addNode(x, y, z);
    eventBus.emit('model:node-added', n);
    eventBus.emit('model:changed');
    return n;
  }
  removeNode(id) {
    this.model.removeNode(id);
    eventBus.emit('model:node-removed', { id });
    eventBus.emit('model:changed');
  }
  addElement(n1, n2) {
    const e = this.model.addElement(n1, n2, this.activeSectionId, this.activeMaterialId);
    eventBus.emit('model:element-added', e);
    eventBus.emit('model:changed');
    return e;
  }
  removeElement(id) {
    this.model.removeElement(id);
    eventBus.emit('model:element-removed', { id });
    eventBus.emit('model:changed');
  }
  addSupport(nodeId, type) {
    const s = this.model.addSupport(nodeId, type);
    eventBus.emit('model:support-added', s);
    eventBus.emit('model:changed');
    return s;
  }
  addNodeLoad(nodeId, Fx, Fy, Fz) {
    const l = this.model.addNodeLoad(nodeId, this.activeLoadCaseId, Fx, Fy, Fz);
    eventBus.emit('model:load-added', l);
    eventBus.emit('model:changed');
    return l;
  }
  setResults(results) {
    this.analysisResults = results;
    eventBus.emit('analysis:complete', results);
  }
  clearResults() {
    this.analysisResults = null;
    eventBus.emit('analysis:cleared');
  }
}

export const appState = new AppState();
