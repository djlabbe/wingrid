<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->

<a name="readme-top"></a>

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/djlabbe/wingrid">
    <img src="images/logo.svg" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">Wingrid</h3>

  <p align="center">
    Pick weekly NFL winners, win bragging rights!
    <br />
    <a href="https://github.com/djlabbe/wingrid"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/djlabbe/wingrid">View Demo</a>
    ·
    <a href="https://github.com/djlabbe/wingrid/issues">Report Bug</a>
    ·
    <a href="https://github.com/djlabbe/wingrid/issues">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <!-- <li><a href="#roadmap">Roadmap</a></li> -->
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About The Project

[![Product Name Screen Shot][product-screenshot]](https://wingrid.io)

Wingrid is in the early stages of development, but will become a platform to play football "pick-em" competitions. Currently, the EventAPI service collects data on all available NFL games from ESPN APIs. Eventually, this data will be used
to generate a subset of games each week for users to predict the winner of. As the games are completed, EventAPI records
the final score and game winner. Wingrid will determine which user correctly predicted the winner for the most games each week.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

To get a local copy up and running follow these steps.

### Prerequisites

Wingrid requires the following tools:

- dotnet 8.0+
  ```sh
  https://dotnet.microsoft.com/en-us/
  ```
- docker
  ```sh
  https://www.docker.com/products/docker-desktop/
  ```

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/djlabbe/wingrid.git
   ```
1. Install cake
   ```sh
   dotnet tool restore
   ```
1. Setup the dev database
   ```sh
   dotnet cake
   ```
   1. Start the Server
   ```sh
   cd Source/Web
   dotnet run
   ```
1. Start the Client App
   ```sh
   cd Source/Web/ClientApp
   npm start
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->

## Usage

With the EventAPI service running, navigate to https://localhost:7002/hangfire to view the Hangfire dashboard and control EventAPI batch jobs. Click on "Recurring Jobs", first execute the Teams job, and then the Events job to populate the database. Subsequent runs of the events job will update any existing events (eg. populating the score once the event is in progress or completed.)

_For more examples, please refer to the [Documentation](https://wingrid.io)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP
## Roadmap

- [ ] Feature 1
- [ ] Feature 2
- [ ] Feature 3
    - [ ] Nested Feature

See the [open issues](https://github.com/djlabbe/wingrid/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p> -->

<!-- CONTACT -->

## Contact

Douglas Labbe - dougjlabbe@gmail.com

Project Link: [https://github.com/djlabbe/wingrid](https://github.com/djlabbe/wingrid)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
<!-- ## Acknowledgments

* []()
* []()
* []()

<p align="right">(<a href="#readme-top">back to top</a>)</p> -->

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo_name.svg?style=for-the-badge
[contributors-url]: https://github.com/djlabbe/wingrid/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo_name.svg?style=for-the-badge
[forks-url]: https://github.com/djlabbe/wingrid/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo_name.svg?style=for-the-badge
[stars-url]: https://github.com/djlabbe/wingrid/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/djlabbe/wingrid/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/djlabbe/wingrid/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/linkedin_username
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com
