using UnityEngine;
using UnityEngine.Video;

public class HighligtSet : MonoBehaviour
{
    [SerializeField] private VideoPlayer highlight = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    private string brap =
        @"https://r15---sn-uxap5nvoxg5-j2is.googlevideo.com/videoplayback?expire=1607021154&ei=At7IX42KFcb0yQWAvb3ACQ&ip=95.197.34.205&id=o-AJEME9eKC3DWc8RLW46FNVTNCANK-zgc0i3IxwjAskwS&itag=22&source=youtube&requiressl=yes&mh=om&mm=31%2C29&mn=sn-uxap5nvoxg5-j2is%2Csn-5goeen76&ms=au%2Crdu&mv=m&mvi=15&pl=16&initcwndbps=1738750&vprv=1&mime=video%2Fmp4&ns=M5rRLddZoxnctNy-i75dtCoF&ratebypass=yes&dur=8.893&lmt=1509075594636253&mt=1606999243&fvip=6&c=WEB&n=pf3DtjBHAtqyWPqGu&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cns%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRQIhAIm9w2hokqD1AaInyjQOgAEqUmYJliag4EIWeU-TT5IJAiAh2fxmN3SwL8o2aKDqjdUBNl5UUnnzXC7KLoa6s1ratw%3D%3D&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps&lsig=AG3C_xAwRQIgQn0WEc2wEJq6Fvj7jkG42ZKAbhWhmuKZYxET5RJ_uxMCIQCR0PD410mTfLtx1b8hregdJe4MVP_sDDCIzlloULKHLQ%3D%3D";

    private string yee =
        @"https://r5---sn-uxap5nvoxg5-j2is.googlevideo.com/videoplayback?expire=1607023656&ei=yOfIX-HUIMPT7QTWwai4Ag&ip=95.197.34.205&id=o-AEvLg1i8K6Iv2A592jvpZxLOk-EXt7rrJAC85BjJ3EKA&itag=18&source=youtube&requiressl=yes&mh=6Q&mm=31%2C29&mn=sn-uxap5nvoxg5-j2is%2Csn-5goeen7d&ms=au%2Crdu&mv=m&mvi=5&pcm2cms=yes&pl=16&initcwndbps=1618750&vprv=1&mime=video%2Fmp4&ns=rPq6Kwrx_hKi9WXXLorT_mcF&gir=yes&clen=390875&ratebypass=yes&dur=9.055&lmt=1575108161677800&mt=1607001650&fvip=5&c=WEB&txp=5431432&n=B1A23d443R9R5d9nx&sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cns%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt&sig=AOq0QJ8wRQIhAPm7r-u626_O9cN61l94gOjkeH0JtaxnBR4Rq1SADoRWAiA9ozOPHZeC5pXk31_pG1cuvxYWYETI5tJvrZ-bswqq1g%3D%3D&lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpcm2cms%2Cpl%2Cinitcwndbps&lsig=AG3C_xAwRQIgR6PSq1EMu9kkI_BboZja0Fg8hhCHJYqmJ2cwNbzYx5oCIQCeWZKOU-bamoJNyL5Xp-V5ZZCan-r3HP_dg9kBuhbfyg%3D%3D";
    public void HighlightPlayBrap()
    {
        spriteRenderer.enabled = true;
        highlight.url = brap;
        highlight.Play();
    }
    
    public void HighlightPlayYee()
    {
        spriteRenderer.enabled = true;
        highlight.url = yee;
        highlight.Play();
    }
}