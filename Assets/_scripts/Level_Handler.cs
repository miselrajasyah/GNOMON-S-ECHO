using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Handler : MonoBehaviour
{
    // There are 10 levels each consisting of
    // 1 opening dialouge
    // 1 puzzle
    // 1 ending dialouge

    public GameObject lvl_select_screen;
    public GameObject canvas;
    public GameObject title;
    public GameObject speech;
    public GameObject submit_button;
    public GameObject goal_box;
    public Sprite[] goal_by_lvl;

    private enum States { PLAY, OPENING, CLOSING, LVL_SELECT, TITLE_SCREEN };

    private Level_Updater lvl_updater;
    private Dot_Handler dot_handler;
    private int current_level;
    private Vector3 center;
    private Vector3 offscreen;
    private int state;

    private const int NUM_LVLS = 10;
    private Vector2[][] dot_pos_by_lvl;
    private string[][] opening_dialouge_by_lvl;
    private int speech_pos;
    private string[][] closing_dialouge_by_lvl;
    private char[][][][] win_condition_by_lvl;
    private List<Vector2> used_dots;
    private List<Vector2> temp_used_dots;

    // Start is called before the first frame update
    void Start()
    {
        dot_handler = GetComponent<Dot_Handler>();
        current_level = -1;
        used_dots = new List<Vector2>();
        temp_used_dots = new List<Vector2>();
        center = new Vector3(9f, 5f, 0f);
        offscreen = new Vector3(9f, 20f, 0f);
        lvl_select_screen.transform.position = offscreen;
        lvl_updater = lvl_select_screen.GetComponent<Level_Updater>();
        lvl_updater.Unlock_lvl(0);

        canvas.SetActive(false);
        submit_button.SetActive(false);
        goal_box.SetActive(false);
        state = (int)States.TITLE_SCREEN;
        title.SetActive(true);
        speech_pos = 0;

        // initialize lvl data!
        Initialize_Opening_Dialouge();
        Initialize_Dot_Positions();
        Initialize_Win_Condition();
        Initialize_Closing_Dialouge();
    }

    void Initialize_Win_Condition()
    {
        win_condition_by_lvl = new char[NUM_LVLS][][][];
        win_condition_by_lvl[0] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x' },
                new char[] { 'o','o','x','x' },
                new char[] { 'o','o','o','x' },
                new char[] { 'o','o','o','o' }
            }
        };
        win_condition_by_lvl[1] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o','o' },
                new char[] { 'o','o','o','o' },
                new char[] { 'o','o','o','o' },
                new char[] { 'o','o','o','o' }
            }
        };
        win_condition_by_lvl[2] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' }
            }
        };
        win_condition_by_lvl[3] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' },
                new char[] { 'o','o','o','o','o' }
            }
        };
        win_condition_by_lvl[4] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o' },
                new char[] { 'o','o' },
                new char[] { 'o','o' }
            }
        };
        win_condition_by_lvl[5] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' },
                new char[] { 'o','o','o' }
            }
        };
        win_condition_by_lvl[6] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x'},
                new char[] { 'o','o','x'},
                new char[] { 'o','o','o'}
            },
            new char[][]
            {
                new char[] { 'o','x','x'},
                new char[] { 'o','o','x'},
                new char[] { 'o','o','o'}
            },
            new char[][]
            {
                new char[] { 'o','x' },
                new char[] { 'o','o' }
            }
        };
        win_condition_by_lvl[7] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x','x','x' },
                new char[] { 'o','o','x','x','x','x' },
                new char[] { 'o','o','o','x','x','x' },
                new char[] { 'o','o','o','o','x','x' },
                new char[] { 'o','o','o','o','o','x' },
                new char[] { 'o','o','o','o','o','o' }
            },
            new char[][]
            {
                new char[] { 'o','x','x','x','x' },
                new char[] { 'o','o','x','x','x' },
                new char[] { 'o','o','o','x','x' },
                new char[] { 'o','o','o','o','x' },
                new char[] { 'o','o','o','o','o' }
            }
        };
        win_condition_by_lvl[8] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x','x','x','x','x' },
                new char[] { 'o','o','x','x','x','x','x','x' },
                new char[] { 'o','o','o','x','x','x','x','x' },
                new char[] { 'o','o','o','o','x','x','x','x' },
                new char[] { 'o','o','o','o','o','x','x','x' },
                new char[] { 'o','o','o','o','o','o','x','x' },
                new char[] { 'o','o','o','o','o','o','o','x' },
                new char[] { 'o','o','o','o','o','o','o','o' }
            }
        };
        win_condition_by_lvl[9] = new char[][][]
        {
            new char[][]
            {
                new char[] { 'o','x','x','x','x' },
                new char[] { 'o','o','x','x','x' },
                new char[] { 'o','o','o','x','x' },
                new char[] { 'o','o','o','o','x' },
                new char[] { 'o','o','o','o','o' }
            }
        };
    }

    void Initialize_Dot_Positions()
    {
        dot_pos_by_lvl = new Vector2[NUM_LVLS][];
        dot_pos_by_lvl[0] = new Vector2[]
        {
            // horizontal line of 10 dots
            new Vector2(1, 9),
            new Vector2(2, 9),
            new Vector2(3, 9),
            new Vector2(4, 9),
            new Vector2(5, 9),
            new Vector2(6, 9),
            new Vector2(7, 9),
            new Vector2(8, 9),
            new Vector2(9, 9),
            new Vector2(10, 9)
        };
        dot_pos_by_lvl[1] = new Vector2[]
        {
            // triangle of height 3
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(3, 7),
            // triangle of height 4
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(7, 7),
            new Vector2(7, 6),
            new Vector2(8, 6),

        };
        dot_pos_by_lvl[2] = new Vector2[]
        {
            // 1
            new Vector2(1, 9),
            // 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            // 5
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5)

        };
        dot_pos_by_lvl[3] = new Vector2[]
        {
            // 3 square
            new Vector2(1, 9),
            new Vector2(2, 9),
            new Vector2(3, 9),
            new Vector2(1, 8),
            new Vector2(2, 8),
            new Vector2(3, 8),
            new Vector2(1, 7),
            new Vector2(2, 7),
            new Vector2(3, 7),

            // 4 square
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(6, 9),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(7, 9),
            new Vector2(7, 8),
            new Vector2(7, 7),
            new Vector2(7, 6),
            new Vector2(8, 9),
            new Vector2(8, 8),
            new Vector2(8, 7),
            new Vector2(8, 6)
        };
        dot_pos_by_lvl[4] = new Vector2[]
        {
            // triangle 1
            new Vector2(1, 9),
            new Vector2(2, 8),
            new Vector2(1, 8),
            // triangle 2
            new Vector2(4, 9),
            new Vector2(5, 8),
            new Vector2(4, 8)
        };
        dot_pos_by_lvl[5] = new Vector2[]
        {
            // 2
            new Vector2(1, 9),
            new Vector2(1, 8),
            // 4
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            // 6
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5),
            new Vector2(5, 4)
        };
        dot_pos_by_lvl[6] = new Vector2[]
        {
            // 15
            // row 1
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(1, 6),
            new Vector2(1, 5),
            // row 2
            new Vector2(2, 9),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(2, 6),
            new Vector2(2, 5),
            // row 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            new Vector2(3, 5),
        };
        dot_pos_by_lvl[7] = new Vector2[]
        {
            // 6^2
            // row 1
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(1, 6),
            new Vector2(1, 5),
            new Vector2(1, 4),
            // row 2
            new Vector2(2, 9),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(2, 6),
            new Vector2(2, 5),
            new Vector2(2, 4),
            // row 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            new Vector2(3, 5),
            new Vector2(3, 4),
            // row 4
            new Vector2(4, 9),
            new Vector2(4, 8),
            new Vector2(4, 7),
            new Vector2(4, 6),
            new Vector2(4, 5),
            new Vector2(4, 4),
            // row 5
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5),
            new Vector2(5, 4),
            // row 6
            new Vector2(6, 9),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(6, 5),
            new Vector2(6, 4),
        };
        dot_pos_by_lvl[8] = new Vector2[]
        {
            // 6^2
            // row 1
            new Vector2(1, 9),
            new Vector2(1, 8),
            new Vector2(1, 7),
            new Vector2(1, 6),
            new Vector2(1, 5),
            new Vector2(1, 4),
            // row 2
            new Vector2(2, 9),
            new Vector2(2, 8),
            new Vector2(2, 7),
            new Vector2(2, 6),
            new Vector2(2, 5),
            new Vector2(2, 4),
            // row 3
            new Vector2(3, 9),
            new Vector2(3, 8),
            new Vector2(3, 7),
            new Vector2(3, 6),
            new Vector2(3, 5),
            new Vector2(3, 4),
            // row 4
            new Vector2(4, 9),
            new Vector2(4, 8),
            new Vector2(4, 7),
            new Vector2(4, 6),
            new Vector2(4, 5),
            new Vector2(4, 4),
            // row 5
            new Vector2(5, 9),
            new Vector2(5, 8),
            new Vector2(5, 7),
            new Vector2(5, 6),
            new Vector2(5, 5),
            new Vector2(5, 4),
            // row 6
            new Vector2(6, 9),
            new Vector2(6, 8),
            new Vector2(6, 7),
            new Vector2(6, 6),
            new Vector2(6, 5),
            new Vector2(6, 4),
        };
        dot_pos_by_lvl[9] = new Vector2[]
        {
            // 3 long hexagon
            new Vector2(3, 9),
            new Vector2(4, 9),
            new Vector2(5, 9),

            new Vector2(2, 8),
            new Vector2(1, 7),
            new Vector2(2, 6),

            new Vector2(6, 8),
            new Vector2(7, 7),
            new Vector2(6, 6),

            new Vector2(3, 5),
            new Vector2(4, 5),
            new Vector2(5, 5),

            // inner hexagon
            new Vector2(3, 7),
            new Vector2(4, 7),
            new Vector2(5, 6),
        };
    }

    void Initialize_Closing_Dialouge()
    {
        closing_dialouge_by_lvl = new string[NUM_LVLS][];
        closing_dialouge_by_lvl[0] = new string[]
        {
            "Kerja bagus!",
            "Segitiga ini disebut tetrakti.",
            "Ini melambangkan empat elemen: api, air, udara, dan bumi."
        };
        closing_dialouge_by_lvl[1] = new string[]
        {
            "Kerja bagus.",
            "Bukankah menakjubkan bagaimana bentuk-bentuk ini bisa menyatu dengan sempurna!"
        };
        closing_dialouge_by_lvl[2] = new string[]
        {
            "Bagus sekali.",
            "Apakah kamu memperhatikan mengapa cara ini selalu berhasil?"
        };
        closing_dialouge_by_lvl[3] = new string[]
        {
            "Sempurna!",
            "Sayangnya, metode ini tidak akan berhasil untuk dua persegi sembarang.",
            "Yang berhasil saya sebut sebagai Tripel Pythagoras.",
            "Persegi 3 dan persegi 4 adalah salah satu contoh paling sederhana dan paling indah!"
        };
        closing_dialouge_by_lvl[4] = new string[]
        {
            "Terlihat bagus!",
            "Bilangan Persegi Panjang juga dapat dianggap sebagai hasil perkalian dari dua bilangan bulat berurutan."
        };
        closing_dialouge_by_lvl[5] = new string[]
        {
            "Luar biasa!",
            "Sering kali ada banyak cara berbeda untuk membentuk bilangan yang sama."
        };
        closing_dialouge_by_lvl[6] = new string[]
        {
            "Kerja bagus!"
        };
        closing_dialouge_by_lvl[7] = new string[]
        {
            "Dikerjakan dengan baik!",
            "Jenis konstruksi seperti ini selalu dapat dibalik."
        };
        closing_dialouge_by_lvl[8] = new string[]
        {
            "Kerja bagus!",
            "Bilangan seperti ini disebut bilangan segitiga persegi dan mereka agak langka.",
            "Bilangan berikutnya seperti ini adalah 1.225 yang merupakan persegi 35 x 35 dan bilangan segitiga dengan tinggi 49."
        };
        closing_dialouge_by_lvl[9] = new string[]
        {
            "Selamat!",
            "Kamu telah mempelajari semua yang aku miliki untuk diajarkan tentang pola bilangan!"
        };
    }

    void Initialize_Opening_Dialouge()
    {
        opening_dialouge_by_lvl = new string[NUM_LVLS][];
        opening_dialouge_by_lvl[0] = new string[]
        {
            "Halo dan selamat datang di kuil para penggemar pola bilangan.",
            "Sebagai pendatang baru, kamu tidak diperbolehkan berbicara selama 3 tahun.",
            "Tapi jangan khawatir, aku punya banyak hal untuk diajarkan tentang angka.",
            "Mari kita mulai dengan bilangan segitiga sederhana",
            "Bilangan segitiga dibentuk dengan menyusun titik-titik",
            "Letakkan satu titik, lalu dua titik, kemudian tiga dan seterusnya di atas satu sama lain untuk membentuk segitiga.",
            "Mulailah dengan menyusun 10 titik ini menjadi bilangan segitiga dengan tinggi 4."
        };
        opening_dialouge_by_lvl[1] = new string[]
        {
            "Selanjutnya kita akan melihat bilangan persegi.",
            "Setiap bilangan persegi dapat dibentuk dengan menyusun ulang dua bilangan segitiga dengan tinggi berurutan.",
            "Bangunlah bilangan persegi 4 x 4 menggunakan segitiga dengan tinggi 3 dan segitiga dengan tinggi 4.",
        };
        opening_dialouge_by_lvl[2] = new string[]
        {
            "Cara lain untuk membentuk semua bilangan persegi adalah dengan menjumlahkan bilangan ganjil.",
            "Dengan mengambil n bilangan ganjil berurutan pertama, kamu dapat membentuk persegi dengan ukuran n x n.",
            "Ambil bilangan 1, 3, dan 5 dan bentuklah persegi 3 x 3."
        };
        opening_dialouge_by_lvl[3] = new string[]
        {
            "Cara yang paling menarik untuk membentuk bilangan persegi yang saya temukan adalah dengan menggabungkan dua persegi yang lebih kecil.",
            "Mari kita gabungkan persegi 3 dan persegi 4 menjadi persegi 5.",
        };
        opening_dialouge_by_lvl[4] = new string[]
        {
            "Ketika kamu menambahkan dua bilangan segitiga yang identik, kamu tidak akan mendapatkan bilangan persegi.",
            "Sebaliknya, kamu akan mendapatkan apa yang saya sebut sebagai bilangan Persegi Panjang.",
            "Bilangan Persegi Panjang 1 lebih tinggi daripada lebarnya.",
            "Gunakan dua bilangan segitiga dengan tinggi 2 untuk membentuk bilangan Persegi Panjang dengan lebar 2."
        };
        opening_dialouge_by_lvl[5] = new string[]
        {
            "Cara lain untuk membentuk bilangan Persegi Panjang adalah dengan bilangan genap.",
            "Dengan mengambil n bilangan genap pertama, kamu dapat membentuk bilangan Persegi Panjang dengan lebar n.",
            "Gunakan 2, 4, dan 6 untuk membentuk bilangan Persegi Panjang dengan lebar 3."
        };
        opening_dialouge_by_lvl[6] = new string[]
        {
            "Setiap bilangan dapat dinyatakan sebagai jumlah dari 3 atau kurang bilangan segitiga.",
            "Bentuklah tiga segitiga yang menyusun bilangan 15."
        };
        opening_dialouge_by_lvl[7] = new string[]
        {
            "Seperti yang telah kita lihat, bilangan persegi dapat dibentuk dari bilangan segitiga berurutan.",
            "Ini berarti kita seharusnya dapat membentuk 2 bilangan segitiga dari bilangan persegi tertentu.",
            "Bentuklah bilangan segitiga berurutan yang menyusun bilangan persegi 6 x 6."
        };
        opening_dialouge_by_lvl[8] = new string[]
        {
            "Ada beberapa bilangan khusus yang sekaligus merupakan bilangan persegi dan bilangan segitiga.",
            "Misalnya bilangan persegi 6 x 6 setara dengan segitiga tinggi 8 yang sama dengan 36.",
            "Tunjukkan ini dengan membentuk segitiga tinggi 8 dari persegi 6 x 6."
        };
        opening_dialouge_by_lvl[9] = new string[]
        {
            "Ada makna di mana setiap pola bilangan memiliki kumpulan bilangan yang sesuai.",
            "Kita telah melihat bilangan segitiga dan persegi, tetapi ada juga bilangan segilima, segienam, dan tak terhingga lainnya.",
            "Mari kita lihat bilangan heksagon. Semua bilangan segienam juga merupakan bilangan segitiga.",
            "Bentuklah bilangan segitiga dari bilangan segitiga ini."
        };
    }

    public void Start_Opening_Speech(int lvl_ID)
    {
        current_level = lvl_ID;
        canvas.SetActive(true);
        lvl_select_screen.transform.position = offscreen;
        speech_pos = 0;
        speech.GetComponent<Text>().text = opening_dialouge_by_lvl[current_level][speech_pos];
        submit_button.SetActive(false);
        goal_box.SetActive(false);

        state = (int)States.OPENING;
    }

    public void Start_Closing_Speech(int lvl_ID)
    {
        current_level = lvl_ID;
        canvas.SetActive(true);
        lvl_select_screen.transform.position = offscreen;
        speech_pos = 0;
        speech.GetComponent<Text>().text = closing_dialouge_by_lvl[current_level][speech_pos];
        speech_pos++;
        submit_button.SetActive(false);
        goal_box.SetActive(false);

        state = (int)States.CLOSING;
    }

    public void Start_lvl(int lvl_ID)
    {
        current_level = lvl_ID;
        dot_handler.place_dots(dot_pos_by_lvl[lvl_ID]);
        dot_handler.play();
        lvl_select_screen.transform.position = offscreen;
        canvas.SetActive(false);
        submit_button.SetActive(true);
        goal_box.SetActive(true);
        goal_box.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = goal_by_lvl[lvl_ID];
        submit_button.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);

        state = (int)States.PLAY;
    }

    public void End_lvl(int lvl_ID)
    {
        current_level = -1;
        dot_handler.stop();
        lvl_select_screen.transform.position = center;
        canvas.SetActive(false);
        submit_button.SetActive(false);
        goal_box.SetActive(false);
        dot_handler.Remove_Dots();

        lvl_updater.Complete_lvl(lvl_ID);
        if (lvl_ID != NUM_LVLS - 1)
        {
            lvl_updater.Unlock_lvl(lvl_ID + 1);
        }

        state = (int)States.LVL_SELECT;
    }

    public void Start_lvl_select(int lvl_ID)
    {
        current_level = -1;
        dot_handler.stop();
        lvl_select_screen.transform.position = center;
        canvas.SetActive(false);
        title.SetActive(false);
        goal_box.SetActive(false);
        submit_button.SetActive(false);

        state = (int)States.LVL_SELECT;
    }

    public bool Check_Answer(char[][] key)
    {
        Vector2[] dot_positions = dot_handler.Get_Dot_Positions();
        bool win = false;
        bool dot_in_position = false;
        foreach (Vector2 pos in dot_positions)
        {
            win = true;
            for (int row = 0; row < key.Length; row++)
            {
                for (int col = 0; col < key[row].Length; col++)
                {
                    float x = pos.x + col;
                    float y = pos.y - row;
                    dot_in_position = false;
                    // win = true if every dot in the win condition corrisponds to a dot in dot_positions
                    foreach (Vector2 pos2 in dot_positions)
                    {
                        if (pos2.x == x && pos2.y == y && !used_dots.Contains(pos2))
                        {
                            dot_in_position = true;
                            temp_used_dots.Add(pos2);
                        }
                    }
                    if (   (!dot_in_position && key[row][col] == 'o') 
                        || (dot_in_position && key[row][col] == 'x' ) )
                    {
                        win = false;
                        temp_used_dots.Clear();
                        break;
                    }
                }
                if (win == false)
                {
                    break;
                }
            }
            if (win == true)
            {
                foreach (var dot in temp_used_dots)
                {
                    used_dots.Add(dot);
                }
                temp_used_dots.Clear();
                return win;
            }
        }
        return win;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == (int)States.OPENING)
        {
            if (Input.GetButtonDown("Fire1") && speech_pos < opening_dialouge_by_lvl[current_level].Length)
            {
                speech.GetComponent<Text>().text = opening_dialouge_by_lvl[current_level][speech_pos];
                speech_pos++;
            }
            else if (Input.GetButtonDown("Fire1") && speech_pos == opening_dialouge_by_lvl[current_level].Length)
            {
                Start_lvl(current_level);
            }
        }
        else if (state == (int)States.CLOSING)
        {
            if (Input.GetButtonDown("Fire1") && speech_pos < closing_dialouge_by_lvl[current_level].Length)
            {
                speech.GetComponent<Text>().text = closing_dialouge_by_lvl[current_level][speech_pos];
                speech_pos++;
            }
            else if (Input.GetButtonDown("Fire1") && speech_pos == closing_dialouge_by_lvl[current_level].Length)
            {
                End_lvl(current_level);
            }
        }
        else if (state == (int)States.PLAY)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Submit")
                    {

                        bool is_correct = true;
                        used_dots.Clear();
                        foreach (var win_condition in win_condition_by_lvl[current_level])
                        {
                            if (!Check_Answer(win_condition))
                                is_correct = false;
                        }
                        if (is_correct)
                        {
                            // win state
                            transform.GetChild(0).GetComponent<AudioSource>().Play();
                            Start_Closing_Speech(current_level);
                        }
                        else
                        {
                            // lose state
                            transform.GetChild(3).GetComponent<AudioSource>().Play();
                        }
                    }
                }
            }
        }
    }
}