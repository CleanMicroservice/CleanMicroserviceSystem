import requests
import platform
import re

GitHubReleaseUrl = "https://github.com/hashicorp/consul/releases/latest"
print(f"Navigating to: {GitHubReleaseUrl}")
response = requests.get(GitHubReleaseUrl)

tempRedirect = response.url
print(f"Consul redirected url: {tempRedirect}")

tagMatch = re.search(r"v(?P<Tag>[\.\d]+)$", tempRedirect)
if not tagMatch:
    raise RuntimeError("Can not find latest version number in redirect url")

latestTag = tagMatch.groupdict()["Tag"]
print(f"Latest tag: {latestTag}")

ResourceUrl = f"https://releases.hashicorp.com/consul/{latestTag}"
print(f"Navigating to: {ResourceUrl}")
response = requests.get(ResourceUrl)

resourceMatches = re.findall(
    r"<a.*?data-os=" "(\S+)" "\sdata-arch=" "(\S+)" "\shref=" "(\S+)" ">(.*)</a>",
    response.text,
)


"""
consul_1.14.3_darwin_amd64.zip             "darwin" - "amd64"
consul_1.14.3_darwin_arm64.zip             "darwin" - "arm64"
consul_1.14.3_freebsd_386.zip             "freebsd" - "386"
consul_1.14.3_freebsd_amd64.zip           "freebsd" - "amd64"
consul_1.14.3_linux_386.zip                 "linux" - "386"
consul_1.14.3_linux_amd64.zip               "linux" - "amd64"
consul_1.14.3_linux_arm.zip                 "linux" - "arm"
consul_1.14.3_linux_arm64.zip               "linux" - "arm64"
consul_1.14.3_solaris_amd64.zip           "solaris" - "amd64"
consul_1.14.3_windows_386.zip             "windows" - "386"
consul_1.14.3_windows_amd64.zip           "windows" - "amd64"
"""
resources = {(match[0], match[1]): (match[2], match[3]) for match in resourceMatches}
for (os, arch), (link, name) in resources.items():
    print(f"{name.ljust(40)} {os.rjust(10)} - {arch.ljust(10)}\t{link}")

systemName = platform.system().lower()
systemArch = platform.architecture()[0]
print(f"System info: {systemName} - {systemArch}")
