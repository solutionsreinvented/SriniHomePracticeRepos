export class Material {
  constructor(id, name = 'Steel S355') {
    this.id = id;
    this.name = name;
    this.E   = 210e9;   // Young's modulus (Pa)
    this.nu  = 0.3;     // Poisson's ratio
    this.rho = 7850;    // Density (kg/m³)
    this.fy  = 355e6;   // Yield strength (Pa)
  }
  get G() { return this.E / (2 * (1 + this.nu)); }
  toJSON() { return { id:this.id, name:this.name, E:this.E, nu:this.nu, rho:this.rho, fy:this.fy }; }
}
