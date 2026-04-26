Let me explain the Phase 1 requirements in detail:
1. The central structure (to which the rake arms will be connected) can be of a frame structure (cage) or a plate construction (tube) or a combination of both.

For cage:

=> The central structure shall have the flexibility to define in segments (along it's height). Each segment by itself can be a cage or tube.
=> A cage can be of a square shape with either it's side dimension is specified or a radius is specified from which side dimension will be deduced.
=> A cage can also be of octagonal shape with it's side dimension (the side of a square) and corner offsets specified from which the octagonal sides will be deduced.
=> If offsets are zero, it will be effectively a square shape. If offsets are specified, it will be effectively an octagonal shape.
=> Within the definition of the octagonal frame, there shall be an option to specify whether next segment to be continued from the vertices i.e., the eight corners of the octagonal frame or from corners. This decides the connectivity with next segment.
=> The segment is basically defined by a start frame and an end frame between which a framework is formed.
=> One of the central structure segment is the segment where all the arms are connected to on all four sides.
=> The start and end frames can be of any shape (square or octagonal).
=> The framework shall be formulated based on this combination of cross section frames of the segment.
=> For the cage segments, remember the frames will be in the cross section as well as in the elevation.

For Tube:
=> The tube shall be of a circular shape and shall be constructed from plate elements.
=> Currently this will be of a circular shape but in future we might extend this to a different shape.
=> For ciruclar shape the diameter (or radius) shall be specified and it can vary at the start and end of the segment. i.e. it can be a frustum (truncated cone). However, there shall be some validation rules to constrain the user from giving wrong data i.e., at the interface of two segments the radius (for tube) or dimensions for cage shall not specified in such a way that it cannot be connected.
=> The mesh generation of the tube is important and hence there shall proper programmatic in-built methodology for the meshing with an option of settings to have a fine grained control over meshing for the user to decide.

=> Usually the preferred mesh is 4-noded quadrilateral plates. However, wherever there is a transition i.e., from larger to smaller or smaller to larger mesh is required there for one or two strips the preferred way to connect is by 3-noded triangular plates.
A couple of important criteria to be considered is a) The meshing shall be optimized (you know a lot of algorithms that can be used) b) The plate elements aspect ratio shall be technically acceptable.

The total central structure can contains segments of cage or tube connected with each other.

Starting from bottom, let's the first segment can be a cage then it can another cage or a tube followed another segment of cage or tube and so on. This continues vertically upwards till the last segment.

The connectivity points dictates how the segments are connected with each other. The connection can be such that connectivity nodes of the top frame are connected to the connectivity nodes of the bottom frame.

If the subsequent segment is a tube, the bottom most nodes generated from meshing of the tube shall include the connectivity nodes of the top frame of the previous segment.
Similarly if the top segment is a tube, the top most nodes generated from meshing of the tube shall include the connectivity nodes of the bottom frame of the next segment.

For the connectivity specifying Vertices mean eight points on the octagonal and Corners mean the 4 main cardinal corners.
However, this is about where to continue the connectivity. But there should an option to close corners (for generating the framing for that particular.
So, if Close Corners option is selected then there will be member formed at the corners also (not just octogonal polygon but additional two members at each corner)


The Node, Beam, Plate classes are available at the following path:
F:\06. ReInvented\MainProjects\SRi.XamlUIThickenerApp\ReInvented.StaadPro.Interop\Entities

The whole STAAD.Pro interop assembly (with lots of other classes and functionality) is available at the following path:
F:\06. ReInvented\MainProjects\SRi.XamlUIThickenerApp\ReInvented.StaadPro.Interop\bin\Debug\ReInvented.StaadPro.Interop.dll

Investigate them and use them in the geometry generation and formulation.
As per the Vector3D etc., classes use the C# in-built types.

This becomes even more complicated when we start working towards the geometry of the rake arms. As there shall be multiple rakes, each with multiple arms connected to the main central structure. That I will explain in Phase 2 and you might need to make some changes at that point but for now I need you to plan the necessary infracture and foundation for this Phase 1 implementation and explain it to me. Once I confirm, you can start working on it.



Should you think something is still not clear, just ask questions and I will clarify and add to this Prompt.md document.


------------------

This is the background for what I have in the current solution (generated by Gemini 3.1).

The implementation and planning is too bad. And the UI design is also utterly disappointing.

I want to revamp everything (include UI) based on the above prompt and I definitely trust you capabilities. Go ahead.