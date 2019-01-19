# Xml Document Transform

Are you using notepad to edit web.config files on production servers?

I use this tool to patch application and web configuration files during deployment as part of a CI/CD pipeline. Visual Studio can do this at build time but I often need to deploy the RELEASE build to multiple environments using different settings and connection strings.

## Usage
Pass input file and transform file as command line arguments, output goes to Console.Out:

```
   xdt Source.config Transform.config
```

Normally you will redirect the output to a new file, save and replace the original

```
   xdt Source.config Transform.config > result.config
   move Source.config Source.config.orig
   move result.config Source.config
```

# Examples and reference
* Try it out using the example files Source.config and Transform.config
* XDT syntax at https://docs.microsoft.com/en-us/previous-versions/aspnet/dd465326(v=vs.110)

## CI/CD
Here's one way to do it:

* Build this project with VS2017 or later
* Include the `xdt.exe` and `Microsoft.Web.XmlTransform.dll` files in your project and add as references
* Include your transform file in the output
* Run the transform from whatever deploy tool you are using