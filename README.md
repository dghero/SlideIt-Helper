# SlideIt Helper
## WPF application to generate Powerpoint Slides

This program generates powerpoint slides in .png format.

The program takes text inputs for the Title and Contents of a slide. All words in the title and the bolded words from the contents are then used to find relevant images using the Unsplash API. Multiple images may then be selected to insert into the slide.

A .env file must be present in the same file as the .exe, the contents of which adhere to the following format:
`
UNSPLASH_ACCESS_KEY=yourkeyhere
`

For more information on the Unsplash API, visit https://unsplash.com/developers
