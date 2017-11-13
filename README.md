# poorly coded comics

I love [Poorly Drawn Lines](http://www.poorlydrawnlines.com),
but I never remember to check the site for updates.
This small task server will periodically check for new comics and,
if found, will then send a text alert. Because laughs.

It also fetches [xkcd](https://xkcd.com)!

**Requirements**

- [X] Fetch and parse XKCD json
- [X] Fetch and parse PDL xml
- [ ] Store metadata for each source's most recent comic
- [ ] If comic is new, send text message
- [ ] Run as a scheduled task, rather than single-use

## Implementation

Thankfully, PDL and XKCD provide different formats.
PDL has an RSS feed with ~10 most recent comics,
whereas XKCD has an endpoint to retrieve JSON metadata
for the single most recent comic.
This provided opportunities to work with both JSON and XML parsing!
