# Dgv Click Cell Color

Your code (and the additional comment below it) indicate that you want the color to change on `CellMouseDown`. It appears that what keeps this from happening is that the cell becomes **selected** the "moment it's clicked". So, if you don't _also_ change the `cell.Style.SelectionBackColor` then it will probably not behave the way you want it to. When I tested it, that small change seems to make it work the way you describe.

[![screenshot][1]][1]

___

Here, when mouse goes to the down state, this method not only sets the `cell.Style.SelectionBackColor`, it also washes it out a little for some visual feedback.

```
private void onCellMouseDown(object? sender, DataGridViewCellMouseEventArgs e)
{
    if(dataGridView.Columns[nameof(Color)].Index == e.ColumnIndex &&  e.RowIndex != -1)
    {
        Color newColor;
        var cell = dataGridView[e.ColumnIndex, e.RowIndex];
        switch (cell.Style.BackColor.Name) 
        {
            case nameof(Color.LightGreen): newColor = Color.LightSalmon; break;
            case nameof(Color.LightSalmon): newColor = Color.LightBlue; break;
            case nameof(Color.LightBlue):
            default:
                newColor = Color.LightGreen;
                break;

        }
        Recordset[e.RowIndex].Color = 
            cell.Style.BackColor = newColor;

        const int OFFSET = 40;
        cell.Style.SelectionBackColor =
            Color.FromArgb(
                Math.Min(255, newColor.R + OFFSET),
                Math.Min(255, newColor.G + OFFSET),
                Math.Min(255, newColor.B + OFFSET)
        );
        dataGridView.Refresh();
    }
}
```

When the mouse comes back up, pull the real `Style.BackColor` and transfer it to `Style.SelectionBackColor`.

```
private void onCellMouseUp(object? sender, DataGridViewCellMouseEventArgs e)
{
    if (dataGridView.Columns[nameof(Color)].Index == e.ColumnIndex && e.RowIndex != -1)
    {
        var cell = dataGridView[e.ColumnIndex, e.RowIndex];

        cell.Style.SelectionBackColor =
            cell.Style.BackColor;
        dataGridView.Refresh();
    }
}
```

And I should mention that I set this column to `ReadOnly` because multiple mouse clicks are going to enter `Edit` mode otherwise and you might have a new problem to deal with.




  [1]: https://i.stack.imgur.com/t0OtS.png