VAR name = "Watson"
VAR intellect = 3
VAR psyche = 3
VAR physique = 3
VAR motorics = 3

VAR met_senje = false
VAR senje_name = ""

== function SKILLCHECK(skill) ==
~ return RANDOM(1 , 6) + RANDOM(1 , 6) + skill