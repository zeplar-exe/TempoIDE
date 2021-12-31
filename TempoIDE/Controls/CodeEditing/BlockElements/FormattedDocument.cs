using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Jammo.ParserTools;
using TempoControls.Core.IntTypes;

namespace TempoIDE.Controls.CodeEditing.BlockElements;

public class FormattedDocument
{
    private readonly List<FormattedString> b_lines;
    private readonly List<Drawing> b_overlayDrawings;
    
    public IEnumerable<FormattedString> Lines => b_lines;
    public int TextLength => Lines.Sum(l => l.Length);
    
    public bool LineWrapping { get; set; }

    public FormattedDocument()
    {
        b_lines = new List<FormattedString>();
        b_overlayDrawings = new List<Drawing>();
    }

    public void AddOverlay(Drawing drawing) => b_overlayDrawings.Add(drawing);
    public bool RemoveOverlay(Drawing drawing) => b_overlayDrawings.Remove(drawing);

    public void UpdateOverlay(IEnumerable<Drawing> overlay)
    {
        b_overlayDrawings.Clear();
        b_overlayDrawings.AddRange(overlay);
    }

    public void AddLine(FormattedString formattedString) => b_lines.Add(formattedString);
    public bool RemoveLine(FormattedString formattedString) => b_lines.Remove(formattedString);

    public void UpdateLines(IEnumerable<FormattedString> lines)
    {
        b_lines.Clear();
        b_lines.AddRange(lines);
    }
    
    public IEnumerable<FormattedCharacter> GetCharactersInRange(IntRange range)
    {
        range = range.Arrange();
        
        return Lines.SelectMany(c => c).Skip(range.Start).Take(1 + range.Size);
    }
    
    public void Draw(DrawingContext context)
    {
        var drawPosition = new Point(0, 0);

        foreach (var overlay in b_overlayDrawings)
        {
            context.DrawDrawing(overlay);
        }
        
        foreach (var line in Lines)
        {
            var formattedLine = line.CreateFormattedText();
            
            context.DrawText(formattedLine, drawPosition);

            foreach (var character in line)
            {
                var formattedCharacterText = character.CreateFormattedText(line.DrawInfo);
                
                foreach (var visual in character.Visuals)
                {
                    if (visual.Position == FormattedVisualPosition.Override)
                    {
                        visual.Visual.Draw(context, visual.OverridenPosition);
                        continue;
                    }
                    
                    Vector positionOffset;
                    
                    switch (visual.Position)
                    { // Note that draw origin is top left, position goes top-bottom, left-right
                        case FormattedVisualPosition.TopLeft:
                            break;
                        case FormattedVisualPosition.TopRight:
                            positionOffset = new Vector(
                                formattedCharacterText.WidthIncludingTrailingWhitespace,
                                0);
                            break;
                        case FormattedVisualPosition.TopCenter:
                            positionOffset = new Vector(
                                formattedCharacterText.WidthIncludingTrailingWhitespace / 2,
                                0);
                            break;
                        case FormattedVisualPosition.BottomLeft:
                            positionOffset = new Vector(
                                0,
                                formattedCharacterText.Height);
                            break;
                        case FormattedVisualPosition.BottomRight:
                            positionOffset = new Vector(
                                formattedCharacterText.WidthIncludingTrailingWhitespace,
                                formattedCharacterText.Height);
                            break;
                        case FormattedVisualPosition.BottomCenter:
                            positionOffset = new Vector(
                                formattedCharacterText.WidthIncludingTrailingWhitespace / 2,
                                formattedCharacterText.Height);
                            break;
                        case FormattedVisualPosition.LeftCenter:
                            positionOffset = new Vector(
                                0,
                                formattedCharacterText.Height / 2);
                            break;
                        case FormattedVisualPosition.RightCenter:
                            positionOffset = new Vector(
                                formattedCharacterText.WidthIncludingTrailingWhitespace,
                                formattedCharacterText.Height / 2);
                            break;
                        case FormattedVisualPosition.Center:
                            positionOffset = new Vector(
                                formattedCharacterText.WidthIncludingTrailingWhitespace / 2,
                                formattedCharacterText.Height / 2);
                            break;
                    }
                    
                    visual.Visual.Draw(context, drawPosition + positionOffset);
                }
                
                drawPosition.X += formattedCharacterText.Width;
            }

            drawPosition.X = 0;
            drawPosition.Y += formattedLine.Height;
        }
    }
}