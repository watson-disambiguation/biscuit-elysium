INCLUDE MasterFile.ink

This is a new text.
-> Main


== Main ==
Which path do you choose, {name}?
* Left path.
    -> First_Knot
* Middle path.
    -> Second_Knot
* Right Path.
    -> Third_Knot
* I give up.
    -> END

== First_Knot ==
    The left path is overgrown with thick vines. You try beating your way through but it is completely impassible. 
    + You can't beat me that easily.
    -> First_Knot
    + I give up.
    -> Main
== Second_Knot ==
    The middle path is  has been covered by an avalanche. As you dig the snow away more crumbles down to fill its place. 
    + You can't beat me that easily.
    -> Second_Knot
    + I give up.
    -> Main
== Third_Knot ==
        The right path leads to a bridge that has collapsed into a ravine. The ravine is deep, and the bridge is too wide to jump across. You try to throw a rope across but can't quite reach. 
    + You can't beat me that easily.
    -> Third_Knot
    + I give up.
    -> Main
