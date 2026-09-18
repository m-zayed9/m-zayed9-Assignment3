// .csproj  = project settings (target framework, output type, packages). MSBuild reads it to build the app.
// obj/ = temporary build files. Can delete, it comes back on next build.
// bin/ = final output (.exe, .dll). This is what i can use to share prject with others as an excutable version.

// Solution format: my project uses .slnx (the new one).
// One advantage of the old .sln: it works with older Visual Studio and old tools, .slnx needs new tools.

// File-scoped namespace: the semicolon applies to the whole file, so no { } block.
// No braces = no extra indentation, the class stays at the left side.
namespace CSharpBasicsAssignment;

internal class Program
{
    static void Main(string[] args)
    {
        RunTypesDemo();
        RunValueVsReferenceDemo();
        RunScopeDemo();
        RunCompoundDemo();
        RunBitwiseDemo();
        RunSingleNumberDemo();
    }

    static void RunTypesDemo()
    {
        // ---------- 1. One variable of every type ----------
        Console.WriteLine("--- 1. Types ---");
        int age = 25;
        long population = 8000000000;
        double pi = 3.14159;
        decimal price = 19.99m;
        bool isStudent = true;
        char grade = 'A';
        string name = "Ahmed";
        var inferred = 2.5f;   // compiler decides the type, here it is float

        Console.WriteLine($"int: {age} -> {age.GetType()}");
        Console.WriteLine($"long: {population} -> {population.GetType()}");
        Console.WriteLine($"double: {pi} -> {pi.GetType()}");
        Console.WriteLine($"decimal: {price} -> {price.GetType()}");
        Console.WriteLine($"bool: {isStudent} -> {isStudent.GetType()}");
        Console.WriteLine($"char: {grade} -> {grade.GetType()}");
        Console.WriteLine($"string: {name} -> {name.GetType()}");
        Console.WriteLine($"var: {inferred} -> {inferred.GetType()}");

        // ---------- 2. Implicit conversion ----------
        Console.WriteLine("\n--- 2. Implicit conversion ---");
        int smallNumber = 100;
        long bigNumber = smallNumber;   // int -> long
        char letter = 'A';
        int letterCode = letter;        // char -> int (gives 65)

        Console.WriteLine($"int to long: {bigNumber}");
        Console.WriteLine($"char to int: {letterCode}");
        // No cast needed because the target type is bigger and can hold every value
        // of the source type.

        // ---------- 3. Explicit conversion ----------
        Console.WriteLine("\n--- 3. Explicit conversion ---");
        double d = 9.99;
        int casted = (int)d;              // 9
        int converted = Convert.ToInt32(d); // 10

        Console.WriteLine($"(int)9.99 = {casted}");
        Console.WriteLine($"Convert.ToInt32(9.99) = {converted}");
        // (int) just cuts the decimal part (truncation), Convert.ToInt32 rounds to nearest.

        // ---------- 4. Integer division trap ----------
        Console.WriteLine("\n--- 4. Integer division ---");
        int intResult = 5 / 2;        // 2
        double doubleResult = 5.0 / 2; // 2.5

        Console.WriteLine($"5 / 2 = {intResult}");
        Console.WriteLine($"5.0 / 2 = {doubleResult}");
        //// If both sides are int the result is int and the .5 is cut(integer division); one double makes it double division.

        // ---------- 5. Boxing / unboxing ----------
        Console.WriteLine("\n--- 5. Boxing / unboxing ---");
        int original = 42;
        object boxed = original;      // boxing: int goes into an object
        Console.WriteLine($"After boxing: {boxed}");

        int unboxed = (int)boxed;     // unboxing: need explicit cast back
        Console.WriteLine($"After unboxing: {unboxed}");

        // ---------- 6. Parsing ----------
        Console.WriteLine("\n--- 6. Parsing ---");
        int parsed = int.Parse("42");
        Console.WriteLine($"int.Parse(\"42\") = {parsed}");

        bool ok = int.TryParse("abc", out int badResult);
        Console.WriteLine($"int.TryParse(\"abc\") succeeded? {ok}");
        if (!ok)
        {
            Console.WriteLine($"Could not parse \"abc\", result is {badResult}");
        }

        // ---------- 7. float -> decimal ----------
        Console.WriteLine("\n--- 7. float to decimal ---");
        float f = 3.14f;
        // decimal wrong = f;   // no implicit conversion from float to decimal
        decimal m = (decimal)f;   // explicit cast works
        Console.WriteLine($"float {f} -> decimal {m}");
        // The compiler refuses because float is binary and approximate, decimal is base-10 and exact,
        // It forces you to write the cast so you know about the risk.
    }


    static void RunValueVsReferenceDemo()
    {
        // ---------- Experiment 1: struct ----------
        Console.WriteLine("--- Experiment 1: struct ---");
        Point p1 = new Point { X = 1, Y = 2 };
        Point p2 = p1;      // copies the values, p2 is a new separate Point
        p2.X = 99;

        Console.WriteLine($"p1.X = {p1.X}");   // 1
        Console.WriteLine($"p2.X = {p2.X}");   // 99
                                               // They are different because a struct is a value type: assignment copies the data,
                                               // so p1 and p2 are two independent copies.

        // ---------- Experiment 2: class ----------
        Console.WriteLine("\n--- Experiment 2: class ---");
        Order o1 = new Order
        {
            OrderId = 101,
            CustomerName = "Sara",
            Quantity = 3,
            UnitPrice = 25.50m,
            IsPaid = false,
            DiscountPercent = 10,
            ShippingCity = "Mansoura",
            Priority = 'H',
            ItemCode = 9000000001L
        };
        o1.CalculateTotal();

        Order o2 = o1;      // copies only the reference (address), same object
        o2.IsPaid = true;

        Console.WriteLine($"o1.IsPaid = {o1.IsPaid}");   // True
        Console.WriteLine($"o2.IsPaid = {o2.IsPaid}");   // True
                                                         // Same because o1 and o2 point to the same Order on the heap, so a change
                                                         // through one name is visible through the other.

        object boxedOrder = o1;             // no boxing, just the same address stored
        Order o3 = (Order)boxedOrder;       // cast back, still the same object
        Console.WriteLine($"ReferenceEquals(o1, o3) = {object.ReferenceEquals(o1, o3)}");   // True

        o2.PrintSummary();   // shows Paid: True, even though we changed it through o2

        // ---------- Explanation ----------
        // Local value types (like p1, p2) live on the stack and hold the data directly.
        // For a reference type, the variable (o1, o2) lives on the stack but it only holds an address,
        // and the real Order object lives on the heap.
        // Assigning a value type copies all the data; assigning a reference type copies only the address.
        // Putting an Order in an object variable does not create a new object, because Order is
        // already a reference type, so object just holds the same address (no boxing).
    }

    struct Point
    {
        public int X;
        public int Y;
    }

    class Order
    {
        public int OrderId;
        public string CustomerName = "";
        public int Quantity;
        public decimal UnitPrice;
        public decimal TotalPrice;
        public bool IsPaid;
        public double DiscountPercent;
        public string ShippingCity = "";
        public char Priority;
        public long ItemCode;

        public void CalculateTotal()
        {
            TotalPrice = Quantity * UnitPrice * (decimal)(1 - DiscountPercent / 100);
        }

        public void PrintSummary()
        {
            Console.WriteLine($"Order {OrderId} | {CustomerName} | Total: {TotalPrice:F2} | Paid: {IsPaid}");
        }
    }
    // ****************************//
    private static int visitCount = 5;

    static void RunScopeDemo()
    {
        Console.WriteLine("--- D1: Scope ---");

        // Field scope
        Console.WriteLine($"Field in RunScopeDemo: {visitCount}");
        ReadFieldAgain();

        // Method scope
        int localNumber = 10;
        Console.WriteLine($"Local variable: {localNumber}");
        // Console.WriteLine(localNumber) inside ReadFieldAgain() would not compile,
        // because localNumber is not visible outside this method.

        // Block scope
        for (int i = 0; i < 3; i++)
        {
            int squared = i * i;   // created again in every loop round
            Console.WriteLine($"i = {i}, squared = {squared}");
        }
        // Console.WriteLine(i);
        // Console.WriteLine(squared);
        // Error: because i and squared are out of scope.
    }

    static void ReadFieldAgain()
    {
        Console.WriteLine($"Field in ReadFieldAgain: {visitCount}");
    }

    static void RunCompoundDemo()
    {
        Console.WriteLine("\n--- D2: Compound operators ---");
        int total = 100;

        total += 20; Console.WriteLine($"total += 20 -> {total}");   // 120
        total -= 30; Console.WriteLine($"total -= 30 -> {total}");   // 90
        total *= 2; Console.WriteLine($"total *= 2  -> {total}");   // 180
        total /= 4; Console.WriteLine($"total /= 4  -> {total}");   // 45
        total %= 4; Console.WriteLine($"total %= 4  -> {total}");   // 1
    }

    static void RunBitwiseDemo()
    {
        Console.WriteLine("\n--- D3: Bitwise ---");
        int a = 12;   // 1100
        int b = 10;   // 1010

        Console.WriteLine($"a & b = {a & b}");   // 8
        Console.WriteLine($"a | b = {a | b}");   // 14
        Console.WriteLine($"a ^ b = {a ^ b}");   // 6
    }

    static int FindSingleNumber(int[] nums)
    {
        int result = 0;
        foreach (int n in nums)
        {
            result ^= n;   // same as result = result ^ n
        }
        return result;

        // Why it works: a ^ a = 0 and a ^ 0 = a, and the order of XOR does not matter.
        // So every number that appears twice cancels itself to 0, and only the single one is left.
    }

    static void RunSingleNumberDemo()
    {
        Console.WriteLine("--- Part F: Single Number ---");

        int[] test1 = { 4, 1, 2, 1, 2 };
        int[] test2 = { 2, 2, 1 };
        int[] test3 = { 7, 3, 7, 5, 5 };

        Console.WriteLine($"[4, 1, 2, 1, 2] -> {FindSingleNumber(test1)}");   // 4
        Console.WriteLine($"[2, 2, 1] -> {FindSingleNumber(test2)}");         // 1
        Console.WriteLine($"[7, 3, 7, 5, 5] -> {FindSingleNumber(test3)}");   // 3
    }
}