# Evodia

## IIS URL Rewrite Module
If it's not possible to access the site, it's possibly related the URL rewrites.
Either comment out the following lines in web.config:

```
<rewrite>
    <rules configSource="config\IISRewriteRules.config" />
</rewrite>
```

Or alternatively, you can install the module from here:
https://www.microsoft.com/en-us/download/details.aspx?id=47337

