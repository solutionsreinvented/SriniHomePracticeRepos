class EventBus {
  constructor() { this._map = {}; }
  on(e, fn) { (this._map[e] = this._map[e] || []).push(fn); return this; }
  off(e, fn) { if (this._map[e]) this._map[e] = this._map[e].filter(f => f !== fn); return this; }
  once(e, fn) { const w = (...a) => { fn(...a); this.off(e, w); }; return this.on(e, w); }
  emit(e, data) { (this._map[e] || []).forEach(fn => fn(data)); return this; }
}
export const eventBus = new EventBus();
