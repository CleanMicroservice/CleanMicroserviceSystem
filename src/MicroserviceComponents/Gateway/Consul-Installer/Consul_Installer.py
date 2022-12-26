import requests
import re

class Resource:
    def __init__(self, os:str, arch:str, link:str, name:str):
        self.os = os
        self.arch = arch
        self.link = link
        self.name = name

    def __str__(self) -> str:
        return f"{self.name} \t\t[{self.os} - {self.arch}] \t\t{self.link}"

GitHubReleaseUrl = "https://github.com/hashicorp/consul/releases/latest"
print(f"Navigating to: {GitHubReleaseUrl}")
response = requests.get(GitHubReleaseUrl)

tempRedirect = response.url
print(f"Consul redirected url: {tempRedirect}")

tagMatch = re.search(r'v(?P<Tag>[\.\d]+)$', tempRedirect)
if not tagMatch:
    raise RuntimeError("Can not find latest version number in redirect url")

latestTag = tagMatch.groupdict()["Tag"]
print(f"Latest tag: {latestTag}")

ResourceUrl = f"https://releases.hashicorp.com/consul/{latestTag}"
print(f"Navigating to: {ResourceUrl}")
response = requests.get(ResourceUrl)

resourceMatches = re.findall(
    r"<a.*?data-os=""(\S+)""\sdata-arch=""(\S+)""\shref=""(\S+)"">(.*)</a>", 
    response.text)

resources = [Resource(match[0], match[1], match[2], match[3]) for match in resourceMatches]
for index in range(len(resources)):
    print(f"[{index}]\t{resources[index]}")