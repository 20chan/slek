# Overview

Strongly typed minimalized OOP language

```ts
module info {
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

    fn loadInfosFromFile(path: str): Info[] {
        json = import(json)
        return json.Reader(path).deserialize(typeof(Info[]))
    }

    var infos: Info[] = loadInfosFromFile("infos.json")
    for (var info: Info in infos)
        info.say()

    fn sayInfos(params infos: Info[]) {
        for (var info: Info in infos)
            info.say()
    }

    sayInfos(infos[0], infos[1])
}
```

