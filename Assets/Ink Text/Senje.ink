INCLUDE MasterFile.ink

{not met_senje: -> INTRODUCTION}
{met_senje: -> GREETING}

== INTRODUCTION ==
~ met_senje = true
#s:SENJE
A slender man with bright golden eyes sits uner a tree, watching the nearby lake. He holds a spear across his lap, next to him is a basket full of freshly caught fish. He shifts his gaze towards you. "Hello, I don't believe we have met. I am called Senje. And yourself?" 
    * Come up with a cool fake name \[Intellect: Challenging\] #s:YOU
    {SKILLCHECK(intellect) > 9: 
    #s:<color=B0F0F4>INTELLECT</color>
    A stroke of genius, the perfect name calls out to you. Plain, yet somewhat interesting. Mistwood.
    ~senje_name = "Mistwood"
    -> say_name
    - else:
    #s:<color=B0F0F4>INTELLECT</color>
    A good name eludes you.
    ~senje_name = name
    -> say_name
    }
    * "I am called {name}" #s:YOU
    ~senje_name = name
    - (excellent) Senje smiles. "An excellent name." #s:SENJE
    -> MAIN
    - (say_name)
    * "I am called {senje_name}" #s:YOU
    -> excellent
== GREETING ==
Senje raises his head towards you. "Hello again, {senje_name}." #s:SENJE
-> MAIN
== MAIN ==
+ "Thank you for your time" \[Leave.\]
-> END