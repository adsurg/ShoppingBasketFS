module Tests

open Xunit

type Product = {name: string; price: decimal}

let emptyBasket : Product list = []

let add (product : Product) (basket : Product list): Product list =
    basket @ [product]

let subTotal (basket : Product list) : decimal =
    basket |> List.sumBy  (fun product -> product.price)

[<Fact>]
let ``When a product is added to an empty basket the total matches the price of the product`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m}
    let total = subTotal basket        
    
    Assert.Equal(39.99m, total)

