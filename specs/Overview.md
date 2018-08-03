# Overview

Strongly typed minimalized OOP language

```ts
struct Info {
    name: str
    age: int

    new(name:str, age:int) {
        this.name = name
        this.age = age
    }
    
    fn say(): void {
        print("My name is ", name, "and I'm ", age, " years old.\n")
    }
}

var infos: Info[] = loadInfosFromFile("infos.json")
for (var info: Info in infos)
    info.say()
```

