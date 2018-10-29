module Tests

open Xunit
open System

type Product = {name: string; price: decimal}

let emptyBasket : Product list = []

let add product (basket : Product list) =
    basket @ [product]

let twoDecimalPlaces(number:decimal) =
    Math.Round(number, 2)

let subTotal (basket : Product list) =
    basket 
        |> List.sumBy (fun product -> product.price)
        |> twoDecimalPlaces

[<Fact>]
let ``When a product is added to an empty basket the total matches the price of the product`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m}
    let total = subTotal basket        
    
    Assert.Equal(39.99m, total)

