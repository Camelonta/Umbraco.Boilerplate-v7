/*
 *  IKONER: http://nicbell.github.io/ucreate/icons.html
 *  DOKUMENTATION: https://our.umbraco.org/documentation/getting-started/backoffice/property-editors/built-in-property-editors/grid-layout/grid-editors
 */

[
	{
	    "name": "Rich text editor",
	    "alias": "rte",
	    "view": "rte",
	    "icon": "icon-article"
	},
	{
	    "name": "Image",
	    "alias": "media",
	    "view": "media",
	    "icon": "icon-picture"
	},
    {
        "name": "Youtube",
        "alias": "Youtubevideo",
        "view": "macro",
        "icon": "icon-play",
        "config": {
            "macroAlias": "Youtubevideo"
        }
    },
	{
	    "name": "Macro",
	    "alias": "macro",
	    "view": "macro",
	    "icon": "icon-settings-alt"
	},
	{
	    "name": "Quote",
	    "alias": "quote",
	    "view": "textstring",
	    "icon": "icon-quote",
	    "config": {
	        "style": "border-left: 3px solid #ccc; padding: 10px; color: #ccc; font-family: serif; font-variant: italic; font-size: 18px",
	        "markup": "<blockquote>#value#</blockquote>"
	    }
	}
]