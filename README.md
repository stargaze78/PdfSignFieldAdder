# PdfSignFieldAdder

**PdfSignFieldAdder** is a simple command-line tool for adding signature fields to PDF documents. 
It uses [iTextSharp](https://github.com/itext/itextsharp) under the AGPL v3 license.

## Features

- Add a visible PDF signature field by specifying coordinates
- Works with any PDF version supported by iTextSharp
- Suitable for integration in document preparation workflows

## Usage

```bash
PdfSignFieldAdder.exe input.pdf output.pdf Signature1 100 700 250 750
```

| Argument          | Description                            |
| ----------------- | -------------------------------------- |
| `input.pdf`       | Path to the input PDF file             |
| `output.pdf`      | Path to the output PDF file            |
| `Signature1`      | Name of the signature field            |
| `100 700 250 750` | Rectangle coordinates (x1, y1, x2, y2) |

## Third-Party Libraries

This tool references the following open-source libraries:

- **iTextSharp** (AGPL v3): PDF reading and modification
- **Fody** (MIT): IL weaving infrastructure
- **Costura.Fody** (MIT): Embeds referenced assemblies as resources

These libraries are included as part of the build or packaging process. 
See LICENSE files or official sources for more details.

## License

This project is licensed under the **GNU Affero General Public License v3.0 (AGPL-3.0)**.
See the [LICENSE](LICENSE) file for details.

> ⚠️ Note: This tool depends on iTextSharp (AGPL v3). Any modified or distributed version must also comply with AGPL.