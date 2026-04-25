export class FEANode {
  constructor(id, x, y, z) {
    this.id = id;
    this.x = x; this.y = y; this.z = z;
    this.label = `N${id}`;
  }
  clone() { return new FEANode(this.id, this.x, this.y, this.z); }
  toJSON() { return { id: this.id, x: this.x, y: this.y, z: this.z, label: this.label }; }
}
