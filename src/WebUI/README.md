# Aphrodite

## Error

### ERR_CERT_AUTHORITY_INVALID

```
Input "chrome://flags/" in Chrome's address bar
Search "Insecure"

Enable "Allow invalid certificates for resources loaded from localhost."
Disable "Block insecure private network requests."
Enable "Insecure origins treated as secure"
	Input: https://10.1.100.73:21000
```

## Workload

```
cd .\WebUI\CleanMicroserviceSystem.Aphrodite\
dotnet workload /h
dotnet workload restore
dotnet workload list
dotnet workload update
dotnet workload clean
dotnet workload repair
```

## Icons

> Home: [Material Design Icons - Icon Library - Pictogrammers](https://pictogrammers.com/library/mdi/)
> 
> Main Repository: [Templarian/MaterialDesign](https://github.com/Templarian/MaterialDesign)
> 
> Distribution Repository: [Templarian/MaterialDesign-Webfont](https://github.com/Templarian/MaterialDesign-Webfont)

### Install

> NPM Package: [@mdi/font](https://www.npmjs.com/package/@mdi/font)

```shell
npm install @mdi/font
```

### Example

```aspnet
<link href="./lib/@mdi/font/materialdesignicons.min.css" rel="stylesheet">
<MIcon Large Color="blue">mdi-cog</MIcon>
```

## Images

> Resources: [Storyset | Freepik](https://www.freepik.com/author/stories)


