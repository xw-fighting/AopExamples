namespace CustomersTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open Customers

[<TestClass>]
type CustomersTests() =

   [<TestMethod>]
   member this.TestA() =
      Assert.Fail()
//      let customer = CustomerRepository.LoadCustomer 1
//
//      Assert.IsNotNull(customer)
