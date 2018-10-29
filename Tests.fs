module Tests

open Xunit
open System

type Product = {name: string; price: decimal; quantity: int}

let emptyBasket : Product list = []

let rationalizeBasket (basket: Product list) =
    basket |> List.groupBy (fun product -> product.name)
           |> List.map (fun (key, products) -> 
                {
                name = key; 
                price = products.Head.price; 
                quantity = products |> List.sumBy (fun product->product.quantity)})

let add product (basket : Product list) =
    basket @ [product]
        |> rationalizeBasket

let twoDecimalPlaces(number:decimal) =
    Math.Round(number, 2)

let subTotal (basket : Product list) =
    basket 
        |> List.sumBy (fun product -> product.price * (decimal)product.quantity)
        |> twoDecimalPlaces


let calculateTax taxRate (basket : Product list) =
    (basket |> subTotal) * taxRate / 100m
        |> twoDecimalPlaces

[<Fact>]
let ``When a product is added to an empty basket the total matches the price of the product`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m; quantity=1}
    let total = subTotal basket        
    
    Assert.Equal(39.99m, total)

[<Fact>]
let ``When a five products are added to an empty basket the total is correct`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m; quantity=5}
    let total = subTotal basket        
    
    Assert.Equal(199.95m, total)

[<Fact>]
let ``When a product is added twice to an empty basket the total is correct`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m; quantity=5}
                    |> add {name="Dove Soap"; price=39.99m; quantity=3}
    let total = subTotal basket        
    
    Assert.Equal(319.92m, total)

[<Fact>]
let ``When a product is added twice to an empty basket the basket contains one entry with a summed quantity`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m; quantity=5}
                    |> add {name="Dove Soap"; price=39.99m; quantity=3}
    
    Assert.Equal(1, basket.Length)
    Assert.Equal({name="Dove Soap"; price=39.99m; quantity=8}, basket.Head)

[<Fact>]
let ``When two products are added tax is calculated correctly`` () =
    let basket = emptyBasket
                    |> add {name="Dove Soap"; price=39.99m; quantity=2}
                    |> add {name="Axe Deo"; price=99.99m; quantity=2}
    
    let tax = basket |> calculateTax 12.5m       
    
    Assert.Equal(35.01m, tax)
