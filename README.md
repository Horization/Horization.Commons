# Horization.Commoms
`Horization.Commons` contains a set of commons utilities.
Which contains:
1. [`Horization.Commons.Collection`](#horizationcommonscollection) —— Collection utilities
2. [`Horization.Commons.Collection.Reactive`](#horizationcommonsreactive) —— Reactive extensions for `Horization.Commons.Collection`

## Horization.Commons.Collection
Contains collection utilities.
For example, the `KeyedListWrapper`:
```csharp
var list = new List<string>;
var keyedList = new KeyedListWrapper<List<string>, int, string>(list, s => s.GetHashCode())
    {
      "hello", "world", "greet"
    };

// now you can use a hash code to index a string in `keyedList`
keyedList["hello".GetHashCode()] // returns "hello"
```
All utilities are wrapper, so they can be unwrapped and return back the original collection.
Consider:
```csharp
var observable = new ObservableListWrapper<List<string>, string>(new List<string>());

var keyedList = new KeyedListWrapper<
    ObservableListWrapper<List<string>, string>, // this reserve the type of the wrapped
    int, string>(observable, s => s.GetHashCode(), true);

((ObservableListWrapper<List<string>, string>)keyedList).Subcribe(...); // works
```

## Horization.Commons.Reactive
Simple reactive implementation for Horization.Commons.Collection
