export class Section {
  constructor(id, name = 'IPE 300') {
    this.id   = id;
    this.name = name;
    // IPE 300 defaults
    this.A    = 53.8e-4;   // m²
    this.Iy   = 8356e-8;   // m⁴ (strong axis)
    this.Iz   = 604e-8;    // m⁴ (weak axis)
    this.J    = 20.1e-8;   // m⁴ (torsion)
    this.Wy   = 557e-6;    // m³
    this.Wz   = 80.5e-6;   // m³
    this.h    = 0.300;     // height m
    this.b    = 0.150;     // flange width m
    this.type = 'I';       // I | RHS | CHS | L | T | Rect | Custom
  }
  toJSON() {
    return { id:this.id, name:this.name, A:this.A, Iy:this.Iy, Iz:this.Iz, J:this.J, type:this.type };
  }
}
