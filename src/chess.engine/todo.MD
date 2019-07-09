# CHESS.ENGINE TODOS

## New test assertions
I've been a NUnit user for many years (MSUnit for many years was hopelessly out of date, and I prefer the (more intuitive to me at least) 'test' nonemclature over xUnits 'facts' & 'theory's).

Recently I came across [Shouldly](https://github.com/shouldly/shouldly) which priortises the information you get when things fail, by outputing the actually code that produced the error. 

This is something I've whined about for a while, some frameworks seem designed around producing some nice text to say everything works, which, ultimately is not that useful and doesn't need a whole framework to do. Usefull detail about failures when they occur is what really matters so I'm going to use `Shouldly` for a while.

## TODOS
* there is a bug with enpassant serialisation encoding/decoding (note-to-self find the other note with the details in)
