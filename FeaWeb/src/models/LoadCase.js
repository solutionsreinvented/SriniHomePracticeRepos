export class LoadCase {
  constructor(id, name = 'LC1', type = 'static') {
    this.id          = id;
    this.name        = name;
    this.type        = type;     // static | wind | seismic | thermal
    this.description = '';
    this.selfWeight  = false;
    this.swFactor    = -1.0;    // self-weight factor (negative = downward)
  }
  toJSON() {
    return { id:this.id, name:this.name, type:this.type,
             description:this.description, selfWeight:this.selfWeight, swFactor:this.swFactor };
  }
}
