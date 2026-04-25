export class FEAElement {
  constructor(id, node1Id, node2Id, sectionId = null, materialId = null) {
    this.id = id;
    this.node1Id = node1Id;
    this.node2Id = node2Id;
    this.sectionId = sectionId;
    this.materialId = materialId;
    this.label = `M${id}`;
    this.type = 'beam'; // beam | shell | truss
    this.releaseStart = { Fx:false,Fy:false,Fz:false,Mx:false,My:false,Mz:false };
    this.releaseEnd   = { Fx:false,Fy:false,Fz:false,Mx:false,My:false,Mz:false };
  }
  toJSON() {
    return { id:this.id, node1Id:this.node1Id, node2Id:this.node2Id,
             sectionId:this.sectionId, materialId:this.materialId, label:this.label, type:this.type };
  }
}
