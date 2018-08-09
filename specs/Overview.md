# Overview

Small strongly typed language

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

생각해볼거
근데 사실 이런거보다는 아무 언어에 대해 디버거를 만든다는게 내게 중요해서 일케 남겨두는거임

- [ ] 상속
- [ ] 제너릭
- [ ] 데이터 은닉
- [ ] 캐스팅

|순위|기호|설명|순서|
|--|--|--|--|
|1|! ~ -|단항|R2L|
|2|* / %||L2R|
|3|+ - ++||L2R|
|4|<< >>|Bitwise shift|L2R|
|5|< > <= >= == !=||L2R|
|6|&|Bitwise AND|L2R|
|7|^|Bitwise XOR|L2R|
|8|\||Bitwise OR|L2R|
|9|&&|Logical AND|L2R|
|10|\|\||Logical OR|L2R|
|11|=||R2L|
