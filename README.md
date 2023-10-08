<br />
<div align="center">
  <img src="https://static.repometr.com/images/sources/3.svg" alt="Logo" width="80" height="80">

  <h3 align="center">Yandex disk uploader</h3>

  <p align="center">
    <a href="https://github.com/EmcTROJ1N/YandexDiskPusher/">View Demo</a>
    ·
    <a href="https://github.com/EmcTROJ1N/YandexDiskPusher/issues">Report Bug</a>
    ·
    <a href="https://github.com/EmcTROJ1N/YandexDiskPusher/issues">Request Feature</a>
  </p>
</div>

<!-- ABOUT THE PROJECT -->
# About the project

This project is a simple C# script that monitors a specified directory for new files, sorts them into subdirectories based on their file extensions, uploads them to a Yandex Disk cloud storage using either the Yandex Disk API or the yandex-disk library, and sends a notification message to a Telegram chat using the TelegramApi library.

## How it works

The script uses the FileWatcher class to monitor the specified directory for new files. When a new file is detected, it is moved to a subdirectory based on its file extension. For example, a file with the extension .jpg will be moved to the images subdirectory.

After all the files have been sorted, the script uses either the Yandex Disk API or the yandex-disk library to upload the sorted files to a Yandex Disk cloud storage. You will need to obtain an access token from Yandex in order to use this feature.

Finally, the script sends a notification message to a Telegram chat using the TelegramApi library. You will need to set up a Telegram bot and obtain an API token in order to use this feature.



### Built With

The creation of the project involved:

| Technology                                                                                                      |
| ----------------------------------------------------------------------------------------------------------------|
| ![YandexDiskApi](https://img.shields.io/badge/YAPI-Yandex%20Disk%20Api-green?style=for-the-badge)               |
| ![TelegramApi](https://img.shields.io/badge/API-Telegram%20Api-blue?style=for-the-badge&logo=telegram)          |
| ![DOTNET](https://img.shields.io/badge/C%23-DOTNET-blue?style=for-the-badge&logo=.Net)                          |

<!-- GETTING STARTED -->
## Getting Started

For this project to work, you need to meet some requirements:

<ol>
  <li>.net framework (.net core)</li>
  <li>Visual Studio (JetBrains Rider or other IDE)</li>
</ol>

<!-- ### Prerequisites

This is an example of how to list things you need to use the software and how to install them.
* npm
  ```sh
  npm install npm@latest -g
  ``` 
No special steps are necessary
-->

### Installation

_Below is an example of how you can instruct your audience on installing and setting up your app. This template doesn't rely on any external dependencies or services._

Clone the repo
   ```sh
   git clone https://github.com/EmcTROJ1N/YandexDiskPusher
   ```
Then create a Configuration.json file in the same folder as the executable, and put all your api keys there in this format:
```
{
  "ObservablePath": "/Desktop/test",
  "YandexToken": "",
  "TelegramBotToken": "",
  "TelegramUserId": ""
}
```

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

<ol>
  <li>Fork the Project</li>
  <li>Commit your Changes (`git commit -m 'Add some AmazingFeature'`)</li>
  <li>Push to the Branch (`git push origin feature/AmazingFeature`)</li>
  <li>Open a Pull Request</li>
</ol>


<!-- CONTACT -->
## Contact

Your Name - [@pokrov1970](https://t.me/pokrov1970) - 19et72@mail.ru

[Project Link](https://github.com/EmcTROJ1N/YandexDiskPusher)



<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Use this space to list resources you find helpful and would like to give credit to. I've included a few of my favorites to kick things off!

* [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
* [Yandex.DiskApi](https://github.com/raidenyn/yandexdisk.client)
