import { eventBus } from './EventBus.js';
export class CommandHistory {
  constructor(limit = 200) { this._undo = []; this._redo = []; this._limit = limit; }
  execute(cmd) {
    cmd.execute();
    this._undo.push(cmd);
    if (this._undo.length > this._limit) this._undo.shift();
    this._redo = [];
    eventBus.emit('history:changed', { canUndo: this.canUndo, canRedo: this.canRedo });
  }
  undo() {
    if (!this.canUndo) return;
    const cmd = this._undo.pop();
    cmd.undo();
    this._redo.push(cmd);
    eventBus.emit('history:changed', { canUndo: this.canUndo, canRedo: this.canRedo });
  }
  redo() {
    if (!this.canRedo) return;
    const cmd = this._redo.pop();
    cmd.execute();
    this._undo.push(cmd);
    eventBus.emit('history:changed', { canUndo: this.canUndo, canRedo: this.canRedo });
  }
  get canUndo() { return this._undo.length > 0; }
  get canRedo() { return this._redo.length > 0; }
  clear() { this._undo = []; this._redo = []; }
}
