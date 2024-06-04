# Генератор целочисленных идентификаторов

Число генерируется по алгоритму [Снежинка](https://en.wikipedia.org/wiki/Snowflake_ID)

## Пример использования

```cs
var generator = new SnowflakeGenerator(1, 1);
Console.Writeline($"new id: {generator.GetId()}");
```
