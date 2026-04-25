namespace FeaLinux.Core.Enums;

public enum AnalysisType { LinearStatic, PDelta, Modal, ResponseSpectrum, TimeHistory, Nonlinear }
public enum LoadType { Dead, Live, Wind, Seismic, Snow, Temperature, PreStress, Moving, Other }
public enum LoadCaseType { Dead, Live, WindX, WindY, SeismicX, SeismicY, SeismicZ, Snow, Temperature, Other }
public enum LoadCombinationType { Ultimate, Serviceability, Seismic }
public enum LoadCombinationMethod { LRFD, ASD, IS_LIMIT_STATE }
public enum MaterialType { Steel, Concrete, Timber, Aluminum, Masonry, Custom }
public enum SectionType { ISection, ChannelSection, TSection, Angle, DoubleAngle, Tube, Pipe, Rectangular, Circular, CustomSection }
public enum SupportType { Fixed, Pinned, RollerX, RollerY, RollerZ, Spring, Custom }
public enum MemberType { Beam, Column, Brace, Truss, Cable, Tendon }
public enum MemberReleaseType { None, StartPinned, EndPinned, BothPinned }
public enum LoadDirection { GlobalX, GlobalY, GlobalZ, LocalX, LocalY, LocalZ, Gravity }
public enum MemberLoadType { Uniform, Trapezoidal, Triangular, PointLoad, Moment }
public enum DesignCode { IS800_2007, IS456_2000, IS1893_2016, IS1904_1986, AISC360_22, ACI318_19, ASCE7_22, EC3_2005, EC2_2004, EC8_2004 }
public enum UnitSystem { SI, Imperial, MixedSI }
public enum ForceUnit { N, kN, MN, lb, kip }
public enum LengthUnit { mm, cm, m, inch, ft }
public enum StressUnit { Pa, kPa, MPa, GPa, psi, ksi }
public enum SelectionMode { None, Select, AddNode, AddMember, AddPlate, AddSupport, AddLoad, Measure }
public enum RenderMode { Wireframe, Solid, Rendered, Transparent }
public enum DiagramType { BendingMomentY, BendingMomentZ, ShearFy, ShearFz, Axial, Torsion, DeformedShape }
public enum PlateType { Shell, Membrane, Plate }
public enum DesignStatus { NotDesigned, Pass, Fail, Error }
public enum AnalysisStatus { NotAnalyzed, Running, Completed, Failed }
