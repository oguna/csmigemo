# csmigemo

[![.NET](https://github.com/oguna/csmigemo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/oguna/csmigemo/actions/workflows/dotnet.yml)
![Nuget](https://img.shields.io/nuget/dt/CsMigemo?label=CsMigemo&logo=nuget&color=blue)
![Nuget](https://img.shields.io/nuget/dt/CsMigemoCore?label=CsMigemoCore&logo=nuget&color=blue)

ローマ字のまま日本語をインクリメンタル検索するためのツールであるMigemoを、C#で実装したものです。

## テスト

```
> dotnet test
```

## ビルド

```
> dotnet build CsMigemo -c Release
```

`CsMigemo\bin\Release\netcoreapp3.1`にビルドされたファイルが生成されます。

## CLIプログラムとして実行

プログラムを実行するには、以下のファイルが同じディレクトリに配置されている必要があります。

- CsMigemo.exe
- CsMigemo.dll
- CsMigemoCore.dll
- CsMigemo.runtimeconfig.json

```
> ./CsMigemo.exe
> kensaku
(kensaku|けんさく|ケンサク|建策|憲[作冊]|検索|献策|研削|羂索|ｋｅｎｓａｋｕ|ｹﾝｻｸ)
```

## ライブラリとして組み込み

以下のDLLファイルを参照に追加します。
NuGet経由でDLLを追加可能です。

- CsMigemo.dll
- CsMigemoCore.dll

標準の辞書ファイルを利用するためにCsMigemo.dllを使用します。
他の辞書ファイルを利用するときは、CsMigemo.dllは必要ありません。

```csharp
Assembly assembly = Assembly.GetExecutingAssembly();
Stream stream = assembly.GetManifestResourceStream("CsMigemo.migemo-compact-dict");
var migemo = new Migemo(stream, RegexOperator.DEFAULT);
Console.WriteLine(migemo.Query("kensaku"));
```

## ライセンス

| プロジェクト | ライセンス |
| ---- | ---- |
| CsMigemo | GNU General Public License v3 |
| CsMigemoCore | 3-clause BSD license |
| CsMigemoTests | 3-clause BSD license |
