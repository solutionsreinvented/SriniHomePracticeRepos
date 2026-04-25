export const LoadType = {
  NODE_FORCE:    'node-force',
  NODE_MOMENT:   'node-moment',
  MEMBER_UDL:    'member-udl',
  MEMBER_POINT:  'member-point',
  MEMBER_TRAP:   'member-trap',
  SELF_WEIGHT:   'self-weight'
};

export class Load {
  constructor(id, loadCaseId, type) {
    this.id         = id;
    this.loadCaseId = loadCaseId;
    this.type       = type;
    // Node loads
    this.nodeId     = null;
    this.Fx = 0; this.Fy = 0; this.Fz = 0;
    this.Mx = 0; this.My = 0; this.Mz = 0;
    // Member loads
    this.elementId  = null;
    this.direction  = 'global-y'; // global-x | global-y | global-z | local-y | local-z
    this.w1 = 0;  // start intensity (kN/m)
    this.w2 = 0;  // end intensity (kN/m)
    this.pos = 0; // position for point load (fraction 0-1)
    this.label = `L${id}`;
  }
  toJSON() {
    return { id:this.id, loadCaseId:this.loadCaseId, type:this.type,
             nodeId:this.nodeId, elementId:this.elementId,
             Fx:this.Fx, Fy:this.Fy, Fz:this.Fz,
             Mx:this.Mx, My:this.My, Mz:this.Mz,
             direction:this.direction, w1:this.w1, w2:this.w2, pos:this.pos };
  }
}
