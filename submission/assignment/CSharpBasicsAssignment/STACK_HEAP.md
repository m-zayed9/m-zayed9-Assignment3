# Stack & Heap Drawings

Using the `Order` class from Part C. The 3 lines of code:

```csharp
Order o1 = new Order { OrderId = 1, CustomerName = "Ali" };   // line 1
Order o2 = o1;                                                 // line 2
o2.IsPaid = true;                                              // line 3
```

Note: the fields I did not set have default values (0, false, ""). `CustomerName` is a `string`,
and string is also a reference type, so that field only holds an address to a string object on the heap.

---

## Diagram 1: after line 1

```
        STACK                            HEAP
  +-----------------+              +------------------------------+
  | o1 = 0x1A2B     | -----------> | Order @ 0x1A2B               |
  +-----------------+              |------------------------------|
                                   | OrderId         = 1          |
                                   | CustomerName    = 0x3C4D     | ---> "Ali" (string object)
                                   | Quantity        = 0          |
                                   | UnitPrice       = 0          |
                                   | TotalPrice      = 0          |
                                   | IsPaid          = false      |
                                   | DiscountPercent = 0          |
                                   | ShippingCity    = ""         |
                                   | Priority        = '\0'       |
                                   | ItemCode        = 0          |
                                   +------------------------------+
```

**What changed:** a new `Order` object was created on the heap, and `o1` on the stack holds only its address (`0x1A2B`), not the data.

---

## Diagram 2: after line 2

```
        STACK                              HEAP
  +-----------------+
  | o2 = 0x1A2B     | ---------+
  +-----------------+          |     +------------------------------+
  | o1 = 0x1A2B     | ---------+---> | Order @ 0x1A2B               |
  +-----------------+                |------------------------------|
                                     | OrderId         = 1          |
                                     | CustomerName    = 0x3C4D     | ---> "Ali" (string object)
                                     | Quantity        = 0          |
                                     | UnitPrice       = 0          |
                                     | TotalPrice      = 0          |
                                     | IsPaid          = false      |
                                     | DiscountPercent = 0          |
                                     | ShippingCity    = ""         |
                                     | Priority        = '\0'       |
                                     | ItemCode        = 0          |
                                     +------------------------------+
```

**What changed:** only a new variable `o2` was added on the stack with the same address; there is still ONE Order on the heap, nothing was copied.

---

## Diagram 3: after line 3

```
        STACK                              HEAP
  +-----------------+
  | o2 = 0x1A2B     | ---------+
  +-----------------+          |     +------------------------------+
  | o1 = 0x1A2B     | ---------+---> | Order @ 0x1A2B               |
  +-----------------+                |------------------------------|
                                     | OrderId         = 1          |
                                     | CustomerName    = 0x3C4D     | ---> "Ali" (string object)
                                     | Quantity        = 0          |
                                     | UnitPrice       = 0          |
                                     | TotalPrice      = 0          |
                                     | IsPaid          = true       |  <--- changed
                                     | DiscountPercent = 0          |
                                     | ShippingCity    = ""         |
                                     | Priority        = '\0'       |
                                     | ItemCode        = 0          |
                                     +------------------------------+
```

**What changed:** `IsPaid` became `true` inside the one heap object, and because `o1` and `o2` both point to it, both see the new value.

---

## What would be different with structs?

If `Order` was a `struct` instead of a `class`, there would be no heap object and no address. The variable would hold the data directly (as a local variable, on the stack). It works like `Point` from Part C:

```csharp
Point p1 = new Point { X = 1, Y = 2 };
Point p2 = p1;     // copies ALL the data into a second box
p2.X = 99;         // only p2 changes
```

```
        STACK
  +-------------+   +-------------+
  | p1          |   | p2          |
  |  X = 1      |   |  X = 99     |
  |  Y = 2      |   |  Y = 2      |
  +-------------+   +-------------+
```

So in the diagrams:

- **Diagram 1:** `o1` would be one big box on the stack containing the 10 fields directly, with no arrow.
- **Diagram 2:** `o2 = o1` would make a second full copy of the 10 fields, not a second arrow.
- **Diagram 3:** `o2.IsPaid = true` would change only `o2`; `o1.IsPaid` would stay `false`.

Small note: the `string` fields inside would still be references to string objects on the heap, only the struct's own data is copied.
Also "structs live on the stack" is true for local variables only. A struct that is a field inside a class lives inside that object on the heap.