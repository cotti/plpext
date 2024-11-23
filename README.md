<p align="center">
  <a href="" rel="noopener">
 <img width=200px height=200px src="docs/plpext.png" alt="Plpext Project logo"></a>
</p>

<h3 align="center">Plpext</h3>

<div align="center">

[![Status](https://img.shields.io/badge/status-active-success.svg)]()
[![GitHub Issues](https://img.shields.io/github/issues/cotti/plpext.svg)](https://github.com/cotti/plpext/issues)
[![GitHub Pull Requests](https://img.shields.io/github/issues-pr/cotti/plpext.svg)](https://github.com/cotti/plpext/pulls)
[![License](https://img.shields.io/badge/license-GPLv3-003300.svg)](/LICENSE)

</div>

---


<p align="center">An application to recover, listen and export audio snippets saved in the long-extinct Messenger Plus Library Pack (.plp) format.</p>

<p align="center"> This is phase II of the personal backscratchers project.</p>

## 📝 Table of Contents


- [📝 Table of Contents](#-table-of-contents)
- [🧐 About ](#-about-)
- [📑 Documentation ](#-documentation-)
- [🏁 Getting Started ](#-getting-started-)
- [🕸️ Prerequisites](#️-prerequisites)
- [⛏️ Built Using ](#️-built-using-)
- [✍️ Authors ](#️-authors-)

## 🧐 About <a name = "about"></a>

Plpext (*good luck spelling that any way you want*) is a simple application to retrieve audio snippets that you may have used in MSN Live Messenger with the *very unofficial* add-on Messenger Plus years ago, and somehow left in your backups as a weird, unreadable `.plp` file. 

It is also a rewrite of the very aptly-named *MessengerPlusSoundBankExtractor* with an improved project design. The `plpext-core` solution now contains everything related to file manipulation and playback, leaving the front-end fully free to do as it wishes.

I am also using a less-problematic implementation of OpenAL, which hopefully will make distribution a whole lot easier.

## 📑 Documentation <a name = "documentation"></a>

[Comments and general documentation/musings on the project](docs/comments.md)

## 🏁 Getting Started <a name = "getting_started"></a>

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See [deployment](#deployment) for notes on how to deploy the project on a live system.

## 🕸️ Prerequisites

Building:
- .NET 8.0

Hopefully that'll be it. Releases should be made self-contained.


## ⛏️ Built Using <a name = "built_using"></a>

- [.NET](https://dot.net/) - Core
- [AvaloniaUI](https://avaloniaui.net/) - GUI

## ✍️ Authors <a name = "authors"></a>

- [@cotti](https://github.com/cotti) | [cotti.com.br](https://cotti.com.br)