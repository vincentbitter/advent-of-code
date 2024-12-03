import os

def lines(filename: str) -> list[str]:
    """
    Reads all lines from a file and returns them as a list of strings.

    :param filename: The name of the file to read from.
    :return: A list of strings, each one corresponding to a line in the file.
    """
    if not os.path.isfile(filename):
        raise FileNotFoundError(f"Input file {filename} not found")

    lines: list[str] = []
    with open(filename, "r") as f:
        while line := f.readline():
            lines.append(line.replace("\n", ""))
    
    return lines


def text_without_breaks(filename: str) -> str:
    """
    Reads all lines from a file and returns them as a single line of text.

    :param filename: The name of the file to read from.
    :return: A string with all content of the file except newlines.
    """
    if not os.path.isfile(filename):
        raise FileNotFoundError(f"Input file {filename} not found")

    text = ""
    with open(filename, "r") as f:
        while line := f.readline():
            text += line.replace("\n", "")
    
    return text
