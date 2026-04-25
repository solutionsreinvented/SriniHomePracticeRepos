import { FEANode }   from './Node.js';
import { FEAElement } from './Element.js';
import { Material }   from './Material.js';
import { Section }    from './Section.js';
import { Support }    from './Support.js';
import { Load }       from './Load.js';
import { LoadCase }   from './LoadCase.js';

export class FEAModel {
  constructor() {
    this.nodes     = new Map();
    this.elements  = new Map();
    this.materials = new Map();
    this.sections  = new Map();
    this.supports  = new Map();
    this.loads     = new Map();
    this.loadCases = new Map();
    this._nid = 1; this._eid = 1; this._mid = 1;
    this._sid = 1; this._spid= 1; this._lid = 1; this._lcid = 1;
    // seed defaults
    this._addDefaultMaterial();
    this._addDefaultSection();
    this._addDefaultLoadCase();
  }
  _addDefaultMaterial() {
    const m = new Material(this._mid++); this.materials.set(m.id, m);
  }
  _addDefaultSection() {
    const s = new Section(this._sid++); this.sections.set(s.id, s);
  }
  _addDefaultLoadCase() {
    const lc = new LoadCase(this._lcid++, 'LC1 - Dead Load', 'static');
    this.loadCases.set(lc.id, lc);
  }

  /* --- Nodes --- */
  addNode(x, y, z) {
    const n = new FEANode(this._nid++, x, y, z);
    this.nodes.set(n.id, n); return n;
  }
  removeNode(id) {
    this.nodes.delete(id);
    // remove connected elements
    for (const [eid, el] of this.elements) {
      if (el.node1Id === id || el.node2Id === id) this.elements.delete(eid);
    }
    for (const [sid, sp] of this.supports) {
      if (sp.nodeId === id) this.supports.delete(sid);
    }
  }

  /* --- Elements --- */
  addElement(node1Id, node2Id, sectionId = 1, materialId = 1) {
    const e = new FEAElement(this._eid++, node1Id, node2Id, sectionId, materialId);
    this.elements.set(e.id, e); return e;
  }
  removeElement(id) { this.elements.delete(id); }

  /* --- Supports --- */
  addSupport(nodeId, type = 'pinned') {
    // one support per node
    for (const [sid, sp] of this.supports) {
      if (sp.nodeId === nodeId) { sp.setPreset(type); return sp; }
    }
    const s = new Support(this._spid++, nodeId, type);
    this.supports.set(s.id, s); return s;
  }
  removeSupport(id) { this.supports.delete(id); }
  getSupportAtNode(nodeId) {
    for (const s of this.supports.values()) if (s.nodeId === nodeId) return s;
    return null;
  }

  /* --- Materials --- */
  addMaterial(name) {
    const m = new Material(this._mid++, name); this.materials.set(m.id, m); return m;
  }

  /* --- Sections --- */
  addSection(name) {
    const s = new Section(this._sid++, name); this.sections.set(s.id, s); return s;
  }

  /* --- Loads --- */
  addNodeLoad(nodeId, loadCaseId, Fx, Fy, Fz, Mx = 0, My = 0, Mz = 0) {
    const l = new Load(this._lid++, loadCaseId, 'node-force');
    l.nodeId = nodeId; l.Fx = Fx; l.Fy = Fy; l.Fz = Fz;
    l.Mx = Mx; l.My = My; l.Mz = Mz;
    this.loads.set(l.id, l); return l;
  }
  addMemberUDL(elementId, loadCaseId, w, direction = 'global-y') {
    const l = new Load(this._lid++, loadCaseId, 'member-udl');
    l.elementId = elementId; l.w1 = w; l.w2 = w; l.direction = direction;
    this.loads.set(l.id, l); return l;
  }
  removeLoad(id) { this.loads.delete(id); }

  /* --- Load Cases --- */
  addLoadCase(name, type = 'static') {
    const lc = new LoadCase(this._lcid++, name, type);
    this.loadCases.set(lc.id, lc); return lc;
  }

  /* --- Serialization --- */
  toJSON() {
    return {
      nodes:     [...this.nodes.values()].map(n => n.toJSON()),
      elements:  [...this.elements.values()].map(e => e.toJSON()),
      materials: [...this.materials.values()].map(m => m.toJSON()),
      sections:  [...this.sections.values()].map(s => s.toJSON()),
      supports:  [...this.supports.values()].map(s => s.toJSON()),
      loads:     [...this.loads.values()].map(l => l.toJSON()),
      loadCases: [...this.loadCases.values()].map(lc => lc.toJSON()),
    };
  }

  get stats() {
    return {
      nodes: this.nodes.size, elements: this.elements.size,
      supports: this.supports.size, loads: this.loads.size
    };
  }
}
