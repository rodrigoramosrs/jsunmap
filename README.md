#JSUnMap 🌐🔍

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

<img align="right" alt="GIF" src="./assets/img/logo.png" width="350" />

## Overview 🚀

**jsunmap** is a powerful tool designed to convert JavaScript source map files into source code with additional features. By reading and parsing `.map` files, it extracts information from the de-minified code, reconstructing file structures and code similar to the original. Moreover, it applies regular expressions to gather data for content analysis, aiding in the identification of potentially sensitive information.

## Features 🛠️

- **Source Code Reconstruction:** Generate code structures resembling the original JavaScript source.
- **De-minification:** Extract information from de-minified code to aid in analysis.
- **Regex Analysis:** Apply regular expressions for identifying potentially sensitive data.
- **File Structure Generation:** Reconstruct the file structure based on the information extracted.

## Example 📖

```shell
jsunmap {FILE_OR_URL_PATH}
```

## License 📝

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Feel free to [report issues](https://github.com/rodrigoramosrs/jsunmap/issues) or [make pull requests](https://github.com/rodrigoramosrsjsunmap/pulls). Happy coding! 👩‍💻🚀
