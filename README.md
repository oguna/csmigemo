# csmigemo

���[�}���̂܂ܓ��{����C���N�������^���������邽�߂̃c�[���ł���Migemo���AC#�Ŏ����������̂ł��B

## �e�X�g

```
> dotnet test
```

## �r���h

```
> dotnet build CsMigemo -c Release
```

`CsMigemo\bin\Release\netcoreapp3.1`�Ƀr���h���ꂽ�t�@�C������������܂��B

## CLI�v���O�����Ƃ��Ď��s

�v���O���������s����ɂ́A�ȉ��̃t�@�C���������f�B���N�g���ɔz�u����Ă���K�v������܂��B

- CsMigemo.exe
- CsMigemo.dll
- CsMigemoCore.dll

```
> ./CsMigemo.exe
> kensaku
(kensaku|���񂳂�|�P���T�N|����|��[���]|����|����|����|㮍�|��������������|�ݻ�)
```

## ���C�u�����Ƃ��đg�ݍ���

�ȉ���DLL�t�@�C�����Q�Ƃɒǉ����܂��B

- CsMigemo.dll
- CsMigemoCore.dll

�W���̎����t�@�C���𗘗p���邽�߂�CsMigemo.dll���g�p���܂��B
���̎����t�@�C���𗘗p����Ƃ��́ACsMigemo.dll�͕K�v����܂���B

```csharp
Assembly assembly = Assembly.GetExecutingAssembly();
Stream stream = assembly.GetManifestResourceStream("CsMigemo.migemo-compact-dict");
var migemo = new Migemo(stream, RegexOperator.DEFAULT);
Console.WriteLine(migemo.Query("kensaku"));
```

## ���C�Z���X

| �v���W�F�N�g | ���C�Z���X |
| ---- | ---- |
| CsMigemo | GNU General Public License v3 |
| CsMigemoCore | 3-clause BSD license |
| CsMigemoTests | 3-clause BSD license |
