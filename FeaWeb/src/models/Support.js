export const SupportType = {
  FIXED:    'fixed',
  PINNED:   'pinned',
  ROLLER_X: 'roller-x',
  ROLLER_Z: 'roller-z',
  SPRING:   'spring',
  CUSTOM:   'custom'
};

const PRESETS = {
  'fixed':    { ux:1, uy:1, uz:1, rx:1, ry:1, rz:1 },
  'pinned':   { ux:1, uy:1, uz:1, rx:0, ry:0, rz:0 },
  'roller-x': { ux:0, uy:1, uz:0, rx:0, ry:0, rz:0 },
  'roller-z': { ux:0, uy:1, uz:0, rx:0, ry:0, rz:0 },
};

export class Support {
  constructor(id, nodeId, type = 'pinned') {
    this.id     = id;
    this.nodeId = nodeId;
    this.type   = type;
    const p = PRESETS[type] || PRESETS['pinned'];
    this.ux = p.ux; this.uy = p.uy; this.uz = p.uz;
    this.rx = p.rx; this.ry = p.ry; this.rz = p.rz;
    this.kx = 0; this.ky = 0; this.kz = 0; // spring stiffnesses
  }
  setPreset(type) {
    this.type = type;
    const p = PRESETS[type];
    if (p) { Object.assign(this, p); }
  }
  toJSON() {
    return { id:this.id, nodeId:this.nodeId, type:this.type,
             ux:this.ux, uy:this.uy, uz:this.uz, rx:this.rx, ry:this.ry, rz:this.rz };
  }
}
