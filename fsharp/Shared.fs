module Shared

type Customer() =
   member this.Id : int = 
      0

module Accessor =

   let ReadEntity<'a when 'a : (new : unit -> 'a)> (query : string) (id : int)  =      
      new 'a()