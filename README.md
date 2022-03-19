# IssueInProgressDaysLabeler #

Track and visualize time your issues spend from assigned to closed state. 
Will help product owners, scrum masters and other people instrested in team successful sprints.
The example project with this visualization is [here](https://github.com/ChipoDeil/IssueInProgressDaysLabeler/projects/1).

## Inputs (bold are required)

* **`labels`** - json array of labels by which issues will be filtered.
* **`github-token`** - just a github token :)
* **`days-mode`** - enum value in range [EveryDay, EveryDayExceptWeekend, RussianCalendar]
  * `EveryDay` - will increment days every day
  * `EveryDayExceptWeekend` - every day except Saturday and Sunday
  * `RussianCalendar` - with the help of [working-calendar](https://github.com/mindbox-moscow/working-calendar), will respect russian official holidays. Any extensions are welcomed.
* **`label-to-increment`** - the label you want to increment quantity in. Should contain `{0}` placeholder for formatting purposes. 
* `auto-cleanup` - cleanup labels from issues in closed state (false by default)
* `since-days` - only issues updated during this last number of days are processed (none restriction by default)

## Usage example

It is located in this repository, in order to use this action you will need to set up [workflow](https://github.com/ChipoDeil/IssueInProgressDaysLabeler/blob/master/.github/workflows/issue-days-labeler.yml), the result of its work is displayed in [project](https://github.com/ChipoDeil/IssueInProgressDaysLabeler/projects/1).

## Contribution

Your contributions are always welcome! All your work should be done in your forked repository. Once you finish your work with corresponding tests, please send us a pull request onto `dev` branch for review.

## License

**IssueInProgressDaysLabeler** is released under [MIT License](http://opensource.org/licenses/MIT)

> The MIT License (MIT)
>
> Copyright (c) 2020 [aliencube.org](https://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
